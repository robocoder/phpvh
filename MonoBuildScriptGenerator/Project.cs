using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoBuildScriptGenerator
{
    public class Project
    {
        public string CSProj { get; set; }
        public string OutFile { get; set; }
        public IEnumerable<string> Reference { get; set; }
        public IEnumerable<string> CompilationSymbols { get; set; }
        public IEnumerable<string> Compile { get; set; }        
        public IEnumerable<string> Copies { get; set; }

        public Project(
            string csproj,
            IEnumerable<string> reference, 
            IEnumerable<string> compile,
            string outFile = null,
            IEnumerable<string> compilationSymbols = null,
            IEnumerable<string> copies = null)
        {
            CSProj = csproj;
            OutFile = outFile;
            Reference = reference;
            Compile = compile;
            CompilationSymbols = compilationSymbols;
            Copies = copies;
        }
    }
}
