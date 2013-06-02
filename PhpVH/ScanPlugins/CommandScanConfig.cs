using Components.Aphid.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace PhpVH.ScanPlugins
{
    [Serializable]
    public class CommandScanConfig
    {
        [AphidProperty("probeName")]
        public string ProbeName { get; set; }

        [AphidProperty("testCases")]
        public string[] TestCases { get; set; }

        [AphidProperty("functions")]
        public string[] Functions { get; set; }
    }
}
