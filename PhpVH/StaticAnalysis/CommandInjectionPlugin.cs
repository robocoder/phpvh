using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace PhpVH.StaticAnalysis
{
    public class CommandInjectionPlugin : InsecureCallPlugin
    {
        protected override IEnumerable<string> GetFunctions()
        {
            var c = new ScanPlugins.CommandScanPlugin(null);
            c.Initialize();
            return c.Config.Functions;
        }

        protected override string[] GetSanitizationFunctions()
        {
            return new[] { "escapeshellarg" };
        }        

        protected override string Name
        {
            get { return "Command injection"; }
        }
    }
}
