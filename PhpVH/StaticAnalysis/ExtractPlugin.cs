using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhpVH.StaticAnalysis
{
    public class ExtractPlugin : InsecureCallPlugin
    {
        protected override IEnumerable<string> GetFunctions()
        {
            return new[] { "extract" };
        }

        protected override string Name
        {
            get { return "Insecure extract usage"; }
        }

        protected override string[] GetSanitizationFunctions()
        {
            return new[]
            {
                "EXTR_SKIP",
                "EXTR_PREFIX_SAME",
                "EXTR_PREFIX_ALL",
            };
        }
    }
}
