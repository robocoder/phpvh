using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhpVH.StaticAnalysis
{
    public class ArbitraryUploadPlugin : InsecureCallPlugin
    {
        protected override IEnumerable<string> GetFunctions()
        {
            return new[] { "move_uploaded_file", "fopen" };
        }

        protected override string Name
        {
            get { return "Arbitrary upload"; }
        }
    }
}
