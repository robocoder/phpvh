using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace PhpVH.ScanPlugins
{
    [Serializable]
    public abstract class PluginConfig
    {
        [XmlElement("FuzzString")]
        public string[] FuzzStrings { get; set; }
    }
}
