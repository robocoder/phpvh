using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhpVH.StaticAnalysis
{
    public class StaticAnalysisAlert
    {
        public string Name { get; set; }
        
        public int Line { get; set; }
        
        public string CodeExcerpt { get; set; }

        public StaticAnalysisAlert(string name, int line, string codeExcerpt)
        {
            Name = name;
            Line = line;
            CodeExcerpt = codeExcerpt;
        }

        public StaticAnalysisAlert()
        {
        }

        public override string ToString()
        {
            return string.Format("{0}\r\n{1}\r\n\r\n", Name, CodeExcerpt);
        }
    }
}
