using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace PhpVH.ScanPlugins
{
    [Serializable]
    public class SqlScanConfig : PluginConfig
    {
        [XmlElement("SqlFunction")]
        public SqlFunction[] Functions { get; set; }
    }
}
