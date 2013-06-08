using Components;
using Components.Aphid.Interpreter;
using Components.ConsolePlus;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ReleasePackager
{
    class Program
    {
        private static Release _currentRelease;

        private static DirectoryInfo outputDir;

        static ReleaseProject GetMainProject(ReleaseProject[] projects)
        {
            return projects.Single(x => x.IsMainProject);
        }

        private static void WriteHeader(string format, params string[] args)
        {
            Cli.WriteHeader(
                string.Format(format, args),
                string.Format(
                    "~{0}~~|{1}~",
                    ConsoleColor.White,
                    ConsoleColor.DarkGreen));
        }

        private static void WriteProgressHeader(string text)
        {
            Cli.WriteLine(" ~Blue~~|White~{0,-" + (Console.BufferWidth - 2) + "}~R~", text);
        }

        private static void LoadSettings()
        {
            WriteHeader("Loading release settings");
            var script = "PhpVH.alx";
            Cli.WriteLine("Executing ~Cyan~{0}~R~", script);
            var interpreter = new AphidInterpreter();
            interpreter.InterpretFile(script);
            _currentRelease = interpreter.GetReturnValue().ConvertTo<Release>();
            var projects = _currentRelease.Projects;
            Cli.WriteLine();
        }

        private static void Build()
        {
            WriteHeader("Building");

            foreach (var p in _currentRelease.Projects)
            {
                p.Clean();
                p.Build();
            }
        }

        private static void CopyBinaries()
        {
            WriteHeader("Copying binaries");
            var mainProject = GetMainProject(_currentRelease.Projects);
            outputDir = new DirectoryInfo(_currentRelease.Output)
                .Combine(mainProject.GetVersion().ToString());

            if (outputDir.Exists)
            {
                Cli.WriteLine("Deleting ~Yellow~{0}~R~", outputDir.FullName);
                outputDir.Delete(true);
            }

            Cli.WriteLine();
            WriteProgressHeader("Copying");

            var progress = new CliProgressBar(_currentRelease.Projects.Length);
            progress.Write();

            _currentRelease.Projects
                .Iter(x =>
                {
                    new DirectoryInfo(x.OutputPath).CopyTo(outputDir.FullName, true);
                    progress.Value++;
                    progress.Write();
                });

            Cli.WriteLine();
            Cli.WriteLine();
        }

        private static void Cleanup()
        {
            WriteHeader("Cleaning up");
            Cli.WriteLine("Searching...");
            var fsos = new List<FileSystemInfo>();

            if (_currentRelease.Cleanup.FilePatterns != null && _currentRelease.Cleanup.FilePatterns.Any())
            {
                fsos.AddRange(
                    outputDir
                        .GetFiles("*", SearchOption.AllDirectories)
                        .Where(x => _currentRelease.Cleanup.FilePatterns.Any(y => Regex.IsMatch(x.FullName, y))));
            }

            if (_currentRelease.Cleanup.Files != null && _currentRelease.Cleanup.Files.Any())
            {
                fsos.AddRange(_currentRelease.Cleanup.Files.Select(x => new FileInfo(Path.Combine(outputDir.FullName, x))));
            }

            if (_currentRelease.Cleanup.Directories != null && _currentRelease.Cleanup.Directories.Any())
            {
                fsos.AddRange(_currentRelease.Cleanup.Directories.Select(x => outputDir.Combine(x)));
            }

            Cli.WriteLine("~Cyan~{0}~R~ files and directories found", fsos.Count);
            Cli.WriteLine();
            WriteProgressHeader("Deleting");

            var progress = new CliProgressBar(fsos.Count);
            progress.Write();

            fsos.Iter(x =>
            {
                if (!x.Exists)
                {
                    progress.Value++;
                    progress.Write();
                    return;
                }

                var dir = x as DirectoryInfo;

                if (dir != null)
                {
                    dir.Delete(true);
                }
                else
                {
                    x.Delete();
                }

                progress.Value++;
                progress.Write();
            });
            Cli.WriteLine();
            Cli.WriteLine();
        }

        private static void ZipRelease()
        {
            WriteHeader("Zipping");
            var name = Path.GetFileNameWithoutExtension(_currentRelease.MainProject);
            var zipName = name + outputDir.Name + ".zip";
            var zipFile = Path.Combine(outputDir.FullName, "..", zipName);

            Cli.WriteLine("Creating ~Cyan~{0}~R~", Path.GetFileName(zipFile));

            if (File.Exists(zipFile))
            {
                Cli.WriteLine("~Yellow~Zip with same name already exists, deleting.~R~");
                File.Delete(zipFile);
            }

            ZipFile.CreateFromDirectory(outputDir.FullName, zipFile);
        }

        static void Main(string[] args)
        {
            LoadSettings();
            Build();
            CopyBinaries();
            Cleanup();
            ZipRelease();            

            Cli.WriteLine("Done\r\n");
        }
    }
}
