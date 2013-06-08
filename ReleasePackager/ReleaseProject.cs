using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ReleasePackager
{
    public class ReleaseProject
    {
        public bool IsMainProject { get; set; }

        public string Name { get; private set; }

        public string AssemblyName { get; private set; }

        public string CsprojFilename { get; private set; }

        public string ProjectDirectory { get; private set; }

        public string OutputPath { get; private set; }

        private ReleaseProject() { }

        public Version GetVersion()
        {
            return Assembly.LoadFile(AssemblyName).GetName().Version;
        }

        public void Clean()
        {
            FileSystemInfoHelper.TryDelete(new DirectoryInfo(OutputPath));
        }

        public void Build()
        {
            MSBuild.Build(CsprojFilename);
        }

        public static ReleaseProject Load(string projectFilename, string mode)
        {
            var projectDir = Path.GetDirectoryName(projectFilename);
            var document = XDocument.Load(projectFilename);
            XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";

            var relativeOutputPath = document
                .Descendants(ns + "OutputPath")
                .SingleOrDefault(x => x.Value.ToLower().Contains(mode.ToLower())).Value;

            var outputType = document.Descendants(ns + "OutputType").SingleOrDefault().Value;
            var outputPath = Path.Combine(projectDir, relativeOutputPath);
            var name = document.Descendants(ns + "AssemblyName").SingleOrDefault().Value;
            var fullName = Path.Combine(outputPath, name) + "." + (outputType.ToLower().Contains("exe") ? "exe" : "dll");
            
            return new ReleaseProject()
            {
                Name = name,
                AssemblyName = fullName,
                CsprojFilename = projectFilename,
                ProjectDirectory = projectDir,
                OutputPath = outputPath,
            };
        }
    }
}
