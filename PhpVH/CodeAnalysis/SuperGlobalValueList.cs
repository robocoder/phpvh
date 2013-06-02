using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml.Serialization;

namespace PhpVH.CodeAnalysis
{
    [Serializable]
    public class SuperGlobalValueList
    {
        [XmlAttribute]
        public string Id { get; set; }

        [XmlElement("NameValuePairs")]
        public List<SuperGlobalNameValuePair> Values { get; private set; }

        public SuperGlobalValueList(string id)
        {
            Id = id;
            Values = new List<SuperGlobalNameValuePair>();
        }
    }
}
