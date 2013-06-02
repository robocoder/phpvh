using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml.Serialization;
using Components;

namespace PhpVH.CodeAnalysis
{
    public class SuperGlobalValueTable : TableBase<string, SuperGlobalValueList>
    {
        [XmlAttribute]
        public string Filename { get; set; }

        [XmlElement("SuperGlobal")]
        public override List<SuperGlobalValueList> Items { get; protected set; }

        public SuperGlobalValueTable()
            : base()
        {
        }

        public SuperGlobalValueTable(string filename)
            : this()
        {
            Filename = filename;
        }

        protected override string GetKey(SuperGlobalValueList element)
        {
            return element.Id;
        }

        protected override SuperGlobalValueList CreateElement(string key)
        {
            return new SuperGlobalValueList(key);
        }
    }
}
