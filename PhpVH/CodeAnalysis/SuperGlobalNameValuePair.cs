using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml.Serialization;

namespace PhpVH.CodeAnalysis
{
    [Serializable]
    public class SuperGlobalNameValuePair
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string Value { get; set; }

        public SuperGlobalNameValuePair()
        {
        }

        public SuperGlobalNameValuePair(string name, string value)
            : this()
        {
            Name = name;
            Value = value;
        }
    }
}
