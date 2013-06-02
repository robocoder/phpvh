using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Components;
using PhpVH.LexicalAnalysis;
using PhpVH.CodeCoverage;
using Components.ConsolePlus;

namespace PhpVH
{
    public class HookCollection : List<Hook>
    {
        public void CreateHandlerFile()
        {
            var hookFileText = new StringBuilder("<?php ");

            foreach (Hook h in this)
                hookFileText.AppendLine(h.ReplacementFunction);

            if (!Program.Config.HookSuperglobals)
            {

                hookFileText.AppendLine(PhpResource.Load("PhpApiOverrides"));
                hookFileText.AppendLine(PhpResource.Load("SuperGlobalOverride"));
            }

            if (Program.Config.DynamicScan)
                hookFileText.AppendLine(PhpResource.Load("DynamicProbes"));

            if (!Program.Config.CodeCoverageNone)
                hookFileText.AppendLine(PhpResource.Load("Annotation"));

            hookFileText.Append(" ?>");

            var hookFile = Program.Config.WebRoot + "\\" + Hook.HookFileName;
            try
            {
                File.WriteAllText(hookFile, hookFileText.ToString());
            }
            catch (UnauthorizedAccessException)
            {
                ScannerCli.DisplayError(string.Format("\r\nError writing hook file to {0}", hookFile));

                Environment.Exit(3);
            }
            
        }

        public void DeleteHandlerFile()
        {
            File.Delete(Program.Config.WebRoot + "\\" + Hook.HookFileName);
        }

        private bool CreateBackup(FileInfo file)
        {
            var backup = new FileInfo(file.FullName + ".phpvhbackup");

            try
            {
                if (backup.Exists)
                {
                    backup.CopyTo(file.FullName, true);
                }
                else
                {
                    file.CopyTo(file.FullName + ".phpvhbackup", true);
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                ScannerCli.DisplayError(string.Format("\r\n\r\nError hooking file\r\n{0}\r\n", ex.Message));

                return false;
            }

            return true;
        }

        public void Set(DirectoryInfo Directory)
        {
            IEnumerable<FileInfo> files = null;
                
            try
            {
                files = Directory
                    .GetFiles("*", SearchOption.AllDirectories)
                    .Where(x =>
                        x.Extension.ToLower() == ".php" ||
                        x.Extension.ToLower() == ".inc");
            }
            catch (UnauthorizedAccessException)
            {
                var error = string.Format("\r\nError enumerating files in {0}", Directory.FullName);
            	ScannerCli.DisplayError(error);
                return;
            }

            var progressBar = new CliProgressBar(files.Count());
            progressBar.Write();

            files.AsyncIter(f =>
            {
                if (!CreateBackup(f))
                {
                    return;
                }

                string php = File.ReadAllText(f.FullName);

                PhpToken[] tokens = null;

                Action getTokens = () => 
                    tokens = PhpParser.StripWhitespaceAndComments(new PhpLexer(php)
                        .GetTokens()
                        .ToArray());

                getTokens();

                lock (Program.PageFieldTable)
                {
                    if (!Program.PageFieldTable.ContainsKey(f.FullName))
                        Program.PageFieldTable.Add(f.FullName, new Dictionary<string, List<string>>());

                    var superglobals = StaticAnalyzer.FindSuperglobalFields(tokens);

                    foreach (var superglobal in superglobals)
                    {
                        if (!Program.PageFieldTable[f.FullName].ContainsKey(superglobal.Key))
                            Program.PageFieldTable[f.FullName].Add(superglobal.Key, new List<string>());

                        
                        var newValues = superglobal.Value
                            .Where(x => !Program.PageFieldTable[f.FullName][superglobal.Key].Contains(x));

                        Program.PageFieldTable[f.FullName][superglobal.Key].AddRange(newValues);
                    }
                }

                lock (Program.FileIncludeTable)
                    Program.FileIncludeTable.Add(f.FullName,
                        PhpParser.GetIncludedFiles(tokens));

                var functions = PhpParser.GetGlobalFunctionCalls(tokens);

                if (Program.Config.CodeCoverageReport > 0)
                {
                    php = ScanMetrics.Default.Annotator.AnnotateCode(f.FullName, php, tokens);                    
                }

                if (!Program.Config.HookSuperglobals)
                    php = PreloadHelper.Patch(php);

                foreach (Hook h in this)
                    h.Set(ref php);

                getTokens();

                string include = "require_once('" +
                    Program.Config.WebRoot.Replace('\\', '/') + "/" + Hook.HookFileName + "');";
                    
                if (tokens.Length >= 2 &&tokens[1].TokenType == PhpTokenType.namespaceKeyword)
                {
                    var eos = tokens.FirstOrDefault(x => x.TokenType == PhpTokenType.EndOfStatement);

                    php = php.Insert(eos.Index + 1, "\r\n" + include + "\r\n");                        
                }
                else
                {
                    php = "<?php " + include + " ?>" + php;
                }

                try
                {
                    File.WriteAllText(f.FullName, php);
                }
                catch (UnauthorizedAccessException ex)
                {
                    ScannerCli.DisplayError(string.Format("\r\n\r\nError hooking file\r\n{0}\r\n", ex.Message));

                    return;
                }
                progressBar.Value++;
                progressBar.Write();
            });
        }

        public void Unset(DirectoryInfo Directory)
        {
            var files = Directory
                .GetFiles("*", SearchOption.AllDirectories)
                .Where(x => x.Extension == ".php" || x.Extension == ".inc");

            var progressBar = new CliProgressBar(files.Count());
            progressBar.Write();

            foreach (FileInfo f in files)
            {
                //try
                //{
                    IOHelper.TryAction(() =>
                    {
                        var backupFile = new FileInfo(f.FullName + ".phpvhbackup");

                        if (backupFile.Exists)
                        {
                            backupFile.CopyTo(f.FullName, true);
                            backupFile.Delete();
                        }
                    }, 512, 0);

                    progressBar.Value++;
                    progressBar.Write();
                //}
                //catch (System.Exception ex)
                //{
                //    ScannerCli.DisplayError(string.Format("\r\nError unhooking {0}:\r\n{1}\r\n\r\n", f.FullName, ex.Message));
                //}                
            }

            Cli.WriteLine();

            //foreach (DirectoryInfo d in Directory.GetDirectories())
            //    Unset(d);
        }
    }
}
