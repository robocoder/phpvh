using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace PhpVH.ScanPlugins
{
    [Serializable]
    public class ArbitraryPhpScanConfig : PluginConfig
    {
        [XmlElement]
        public string FalsePositiveRegex { get; set; }

        [XmlElement]
        public string MatchRegex { get; set; }
    }
}
