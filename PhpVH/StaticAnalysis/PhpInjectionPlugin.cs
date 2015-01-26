using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhpVH.StaticAnalysis
{
    public class PhpInjectionPlugin : InsecureCallPlugin
    {
        protected override IEnumerable<string> GetFunctions()
        {
            return new[] { PhpName.Eval };
        }

        protected override string[] GetSanitizationFunctions()
        {
            return new[] { PhpName.HtmlEntities };
        }

        protected override string Name
        {
            get { return "PHP injection"; }
        }
    }
}
