using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml.Serialization;

namespace PhpVH.ScanPlugins
{
    [Serializable]
    public class SqlFunction
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public int ParamCount { get; set; }

        [XmlAttribute]
        public int QueryParam { get; set; }

        public SqlFunction()
        {
        }

        public SqlFunction(string name, int paramCount, int queryParam)
            : this()
        {
            Name = name;
            ParamCount = paramCount;
            QueryParam = queryParam;
        }
    }
}
