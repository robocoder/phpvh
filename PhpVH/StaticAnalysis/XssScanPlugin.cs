using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhpVH.StaticAnalysis
{
    public class XssScanPlugin : InsecureCallPlugin
    {
        protected override IEnumerable<string> GetFunctions()
        {
            return new[]
            {
                "echo",
                "print",
                "printf",
                "vprintf",
            };
        }

        protected override string Name
        {
            get { return "Cross-site scripting"; }
        }

        protected override string[] GetSanitizationFunctions()
        {
            return new[]
            {
                "htmlentities",
                "htmlspecialchars",
            };
        }
    }
}
