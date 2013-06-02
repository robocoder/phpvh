using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoBuildScriptGenerator
{
    public class WindowsBuildScriptGenerator : BuildScriptGenerator
    {
        private string CreateRefArg(Project project)
        {
            return string.Join(" ", project.Reference.Select(x => "-r:" + x));
        }

        private string CreateCompileArgs(Project project)
        {
            return string.Join(" ", project.Compile.Select(x => "\"" + x + "\""));
        }

        private string CreateMkDirs(IEnumerable<string> directories)
        {
            return string.Join(
                "\r\n", 
                directories.Select(x => string.Format("IF NOT EXIST \"{0}\" MKDIR \"{0}\"", x)));
        }

        private string CreateCopies(IEnumerable<Tuple<string, string>> copies)
        {
            return string.Join(
                "\r\n",
                copies.Select(x => string.Format("COPY \"{0}\" \"{1}\"", x.Item1, x.Item2)));
        }

        public override string CreateScript(Project project)
        {
            var projDir = Path.GetDirectoryName(project.CSProj);
            var outDir = Path.GetDirectoryName(project.OutFile);
            var copies = project.Copies.Select(x => Tuple.Create(
                Path.Combine(projDir, x),
                Path.Combine(outDir, x)));
            var allOutDirs = copies.Select(x => Path.GetDirectoryName(x.Item2)).Distinct();
            

            return string.Format(
                "{0}\r\n\r\ncall mcs {1} -unsafe {2} -out:{3} -d:DEBUG -d:TRACE\r\n\r\n{4}\r\n\r\n{5}",
                CreateMkDirs(new[] { outDir }),
                CreateCompileArgs(project),
                CreateRefArg(project),
                project.OutFile,
                CreateMkDirs(allOutDirs),
                CreateCopies(copies));
        }
    }
}
