using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Components;

namespace PhpVH
{
    [Serializable]
    public class ReportFile
    {
        public string Name { get; set; }

        public string Filename { get; set; }

        public ReportFile() { }

        public ReportFile(string name, string filename)
        {
            Name = name;
            Filename = filename;
        }

        public static ReportFile[] Load(string filename)
        {
            return new XmlSerializer(typeof(ReportFile[])).Deserialize(filename) 
                as ReportFile[];
        }
    }
}
