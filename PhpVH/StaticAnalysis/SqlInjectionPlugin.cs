using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhpVH.LexicalAnalysis;

namespace PhpVH.StaticAnalysis
{
    public class SqlInjectionPlugin : InsecureCallPlugin
    {
        protected override IEnumerable<string> GetFunctions()
        {
            var plugin = new PhpVH.ScanPlugins.SqlScanPlugin(null);
            plugin.Initialize();

            return plugin.Config.Functions.Select(x => x.Name);
        }

        protected override string[] GetSanitizationFunctions()
        {
            return new[]
            {
                "mysql_escape_string",
                "mysqli_escape_string",
                "sqlite_escape_string",
                "mysql_real_escape_string",
                "pg_escape_string",
                "addslashes"
            };
        }

        protected override string Name
        {
            get { return "SQL Injection"; }
        }
    }
}
