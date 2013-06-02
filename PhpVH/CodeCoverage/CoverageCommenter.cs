using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Components;
using System.IO;
using Components.ConsolePlus;
using System.Web;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace PhpVH.CodeCoverage
{
    public class CoverageCommenter
    {
        private PluginAnnotationTable _table;

        public CoverageCommenter(PluginAnnotationTable table)
        {
            _table = table;
        }

        public void LoadTable(string file)
        {
            _table = PluginAnnotationTable.Load(file);
        }

        private static string GetCodeCoveragePath(string outputFolder)
        {
            return Path.Combine(outputFolder, "Code Coverage");
        }

        public static string GetCommentedFilePath(string outputFolder, string pluginName)
        {
            return Path.Combine(GetCodeCoveragePath(outputFolder), PathHelper.SanitizeName(pluginName));
        }

        private string CreateComment(Annotation annotation)
        {
            return string.Format(
                "\r\n/*\r\n{0}\r\nhit count: {1}\r\n{0}\r\n*/\r\n",
                new string('-', 32),
                annotation.HitCount);
        }

        private string CreateCommentedCode(AnnotationList annotations)
        {
            var file = annotations.Filename + ".phpvhbackup";

            if (!File.Exists(file))
            {
                Cli.WriteLine("Could not find ~Red~{0}~R~", file);
                return null;
            }

            var code = File.ReadAllText(file);

            var isPhpless = Regex.Matches(code, @"<\?[^=]").Count == 0;

            string commentedCode;

            if (isPhpless)
            {
                var annot = new Annotation() { HitCount = annotations.Items.First().HitCount };
                commentedCode = CreateComment(annot) + code;                
            }
            else
            {
                commentedCode = annotations.Items
                    .OrderByDescending(z => z.Index)
                    .Select(z => new
                    {
                        Index = z.Index,
                        Comment = CreateComment(z)
                    })
                    .Where(z => z.Index < code.Length)
                    .Aggregate(
                        new StringBuilder(code),
                        (sb, z) => sb.Insert(z.Index, z.Comment))
                    .ToString();
            }

            return commentedCode;
        }

        private string GetPageTemplate()
        {
            var templateFilename = PathHelper.GetEntryPath("CodeCoverage", "CoveragePageTemplate.html");

            if (!File.Exists(templateFilename))
            {
                return null;
            }

            return File.ReadAllText(templateFilename);
        }

        static string GetPageFilename(AnnotationList list)
        {
            var md5 = new MD5CryptoServiceProvider();
            return md5.ComputeHash(list.Filename) + ".html";
        }

        static string GetRelativeCodePath(AnnotationList list)
        {
            return list.Filename.Substring(Program.Config.WebRoot.Length + 1);
        }

        public void WriteCommentedFiles(string outputFolder)
        {
            var pageTemplate = GetPageTemplate();

            if (pageTemplate == null)
            {
                Cli.WriteLine("~Red~Could not read code coverage page template~R~");
                return;
            }

            var codePageTable = new CodePageTable();

            var coverageDir = GetCodeCoveragePath(outputFolder);

            _table.Items.Iter(x =>
            {
                Cli.WriteLine("Writing coverage comments for ~Cyan~{0}~R~", x.Plugin);
                
                var pluginDir = GetCommentedFilePath(outputFolder, x.Plugin);
                var relativePluginDir = pluginDir.Substring(coverageDir.Length + 1);

                Directory.CreateDirectory(pluginDir);

                var codePages = new List<CodePage>();

                x.Items.Iter(y =>
                {
                    var commentedCode = CreateCommentedCode(y);

                    if (commentedCode == null)
                    {
                        return;
                    }

                    var codeFilename = GetRelativeCodePath(y);
                    
                    var page = string.Format(
                        pageTemplate, 
                        codeFilename, 
                        HttpUtility.HtmlEncode(x.Plugin),
                        HttpUtility.HtmlEncode(commentedCode));

                    var pageFilename = Path.Combine(pluginDir, GetPageFilename(y));

                    File.WriteAllText(pageFilename, page);

                    var relateivePageFilename = Path.Combine(relativePluginDir, GetPageFilename(y));

                    codePages.Add(new CodePage(codeFilename, relateivePageFilename));

                    Cli.WriteLine("[~Green~+~R~] {0}", codeFilename);

                });

                codePageTable.Add(x.Plugin, codePages);

                Cli.WriteLine();
            });

            var index = Path.Combine(coverageDir, "index.html");
            
            File.WriteAllText(index, codePageTable.ToHtml());

            var highlighterDir = new DirectoryInfo(PathHelper.GetEntryPath("SyntaxHighlighter"));
            highlighterDir.CopyTo(coverageDir);
        }
    }
}
