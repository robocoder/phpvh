using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Components;

namespace PhpVH.StaticAnalysis
{
    public class StaticAnalysisFileAlertCollection : List<StaticAnalysisFileAlerts>
    {
        private static XmlSerializer _serializer = new XmlSerializer(typeof(StaticAnalysisFileAlertCollection));

        public StaticAnalysisFileAlertCollection(IEnumerable<StaticAnalysisFileAlerts> items)
            : base(items)
        {
        }

        public StaticAnalysisFileAlertCollection()
        {
        }

        public string ToXml()
        {
            foreach (var a in this)
            {
                foreach (var alert in a.Alerts)
                {
                    for (int c = (char)0; c < 0x20; c++)
                    {
                        if (c == (int)'\r' || c == (int)'\n')
                            continue;

                        alert.CodeExcerpt = alert.CodeExcerpt.Replace((char)c, ' ');
                    }

                    for (int c = 0x7f; c < 0x10000; c++)
                        alert.CodeExcerpt = alert.CodeExcerpt.Replace((char)c, ' ');
                }
            }

            var xml = _serializer.Serialize(this);

            return xml;
        }

        public static StaticAnalysisFileAlertCollection Load(string filename)
        {
            return _serializer.Deserialize(filename) as StaticAnalysisFileAlertCollection;
        }
    }
}
