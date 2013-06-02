using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Components;

namespace PhpVH.CodeCoverage
{
    [Serializable]
    public class AnnotationTable : TableBase<string, AnnotationList>, ICloneable
    {
        [XmlAttribute]
        public string Plugin { get; set; }

        [XmlElement("FileAnnotations")]
        public override List<AnnotationList> Items { get; protected set; }        

        public AnnotationTable()
        {
            Items = new List<AnnotationList>();
        }

        public AnnotationTable(string plugin)
            : this()
        {
            Plugin = plugin;
        }

        public void Add(string filename)
        {
            Add(new AnnotationList(filename));            
        }

        protected override string GetKey(AnnotationList element)
        {
            return element.Filename;
        }

        protected override AnnotationList CreateElement(string key)
        {
            return new AnnotationList(key);
        }

        public object Clone()
        {
            var annotationTableClone = new AnnotationTable(Plugin);

            foreach (var annotationList in Items)
            {
                var annotationListClone = new AnnotationList(annotationList.Filename);

                annotationTableClone.Items.Add(annotationListClone);

                foreach (var annotation in annotationList.Items)
                {
                    var annotationClone = new Annotation(annotation.Id, annotation.Index);

                    annotationListClone.Add(annotationClone);
                }
            }
            return annotationTableClone;
        }
    }
}
