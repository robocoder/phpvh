using System;
using System.Xml.Serialization;

namespace PhpVH.CodeCoverage
{
    [Serializable]
    public class Annotation
    {
        [XmlAttribute]
        public int Id { get; set; }

        [XmlAttribute]
        public int Index { get; set; }

        [XmlAttribute]
        public int HitCount { get; set; }

        public Annotation(int id, int index, int hitCount)
        {
            Id = id;
            Index = index;
            HitCount = hitCount;
        }

        public Annotation(int id, int index)
            : this(id, index, 0)
        {
        }

        public Annotation()
            : this(0, 0, 0)
        {
        }

        public override string ToString()
        {
            return string.Format(
                "Annotation Id {0}, Index {1}, HitCount {2}",
                Id, Index, HitCount);
        }

        public override bool Equals(object obj)
        {
            var o = (Annotation) obj;
            return Id == o.Id && Index == o.Index && HitCount == o.HitCount;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^ Index.GetHashCode() ^ HitCount.GetHashCode();
        }
    }
}
