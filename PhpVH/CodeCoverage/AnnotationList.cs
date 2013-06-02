using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Components;

namespace PhpVH.CodeCoverage
{
    [Serializable]
    [XmlRoot("AnnotationList")]
    public class AnnotationList : TableBase<int, Annotation>
    {
        [XmlAttribute]
        public string Filename { get; set; }

        [XmlElement("Annotation")]
        public override List<Annotation> Items { get; protected set; }
        
        public AnnotationList(string file)
            : this()
        {
            Filename = file;
        }

        public AnnotationList()
        {
            Items = new List<Annotation>();
        }

        protected override int GetKey(Annotation element)
        {
            return element.Id;
        }

        protected override Annotation CreateElement(int key)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            var o = (AnnotationList)obj;
            return Filename == o.Filename && Items.SequenceEqual(o.Items);
        }

        public override int GetHashCode()
        {
            return Filename.GetHashCode() ^ Items.GetHashCode();
        }
    }
}
