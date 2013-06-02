using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhpVH.StaticAnalysis
{
    public class LfiPlugin : InsecureCallPlugin
    {
        protected override IEnumerable<string> GetFunctions()
        {
            return new[] 
            { 
                "require",
                "require_once",
                "include",
                "include_once",            
            };
        }

        protected override string Name
        {
            get { return "Local file inclusion"; }
        }
    }
}
