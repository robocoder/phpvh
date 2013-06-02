using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhpVH.CodeCoverage
{
    public class CodePage
    {
        public string Name { get; set; }
        public string Filename { get; set; }

        public CodePage()
        {
        }

        public CodePage(string name, string filename)
        {
            Name = name;
            Filename = filename;
        }
    }
}
