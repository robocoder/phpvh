using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MonoBuildScriptGenerator
{
    class Program
    {
        private static XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";

        static IEnumerable<string> GetIncludes(IEnumerable<XElement> itemGroups, string name)
        {
            return itemGroups
                .SelectMany(x => x.Elements(ns + name).Attributes("Include"))
                .Where(x => x != null)
                .Select(x => x.Value);
        }

        static IEnumerable<XElement> GetItemGroups(XDocument document)
        {
            return document.Root.Elements(ns + "ItemGroup");
        }

        static Project LoadProject(string csproj)
        {
            var path = Path.GetDirectoryName(csproj);            
            var doc = XDocument.Load(csproj);
            var itemGroups = GetItemGroups(doc);
            return new Project(
                csproj,
                GetIncludes(itemGroups, "Reference"),
                GetIncludes(itemGroups, "Compile")
                    .Select(x => Path.IsPathRooted(x) ? x : Path.Combine(path, x)), 
                null, 
                null,
                GetIncludes(itemGroups, "None"));
        }

        static string CreateRefArg(Project project)
        {
            return string.Join(" ", project.Reference.Select(x => "-r:" + x));
        }

        static string CreateCompileArgs(Project project)
        {
            return string.Join(" ", project.Compile.Select(x => "\"" + x + "\""));
        }

        static void Main(string[] args)
        {
            var projects = args[0]
                .Split(',')
                .Select(LoadProject)
                .ToArray();

            var outFile = args[1];
            var script = args[2];
            var mainProject = projects.First();
            var asmInfo = @"\assemblyinfo.cs";

            var assemblyInfo = mainProject.Compile
                .FirstOrDefault(x => x.ToLower().EndsWith(asmInfo));            

            var mergedReference = projects.SelectMany(x => x.Reference).Distinct();
            var mergedCompile = projects
                .SelectMany(x => x.Compile)
                .Where(x => !x.ToLower().EndsWith(asmInfo) || x == assemblyInfo)
                .Distinct();
            var mergedCopies = projects.SelectMany(x => x.Copies).Distinct();

            var mergedProject = new Project(
                mainProject.CSProj,
                mergedReference, 
                mergedCompile, 
                outFile,
                new [] { "MONO", "DEBUG", "TRACE" },
                mergedCopies);

            var generator = new WindowsBuildScriptGenerator();
            var cmd = generator.CreateScript(mergedProject);

            File.WriteAllText(script, cmd);
        }
    }
}
