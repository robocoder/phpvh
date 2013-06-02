using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml.Serialization;
using Components;

namespace PhpVH.CodeCoverage
{
    [Serializable]
    public class PluginAnnotationTable : TableBase<string, AnnotationTable>
    {
        private static XmlSerializer _xmlSerializer = new XmlSerializer(typeof(PluginAnnotationTable));
        public PluginAnnotationTable()
        {
            Items = new List<AnnotationTable>();
        }

        [XmlElement("PluginAnnotations")]
        public override List<AnnotationTable> Items { get; protected set; }

        protected override string GetKey(AnnotationTable element)
        {
            return element.Plugin;
        }

        protected override AnnotationTable CreateElement(string key)
        {
            return new AnnotationTable(key);
        }

        public string ToXml()
        {
            
            return _xmlSerializer.Serialize(this);
        }

        public static PluginAnnotationTable Load(string filename)
        {
            return _xmlSerializer.Deserialize(filename) as PluginAnnotationTable;
        }
    }
}
