using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using Components;

namespace PhpVH
{
    [Serializable]
    public class ApplicationMap
    {
        [XmlElement("Page")]
        public List<ApplicationMapPage> Pages { get; set; }
        
        public ApplicationMap()
        {
            Pages = new List<ApplicationMapPage>();
        }

        public void AddTrace(FileTrace Trace)
        {
            if (!Trace.Calls.Any())
                return;

            var page = Pages.SingleOrDefault(x => x.Page == Trace.File);

            if (page == null)
            {
                page = new ApplicationMapPage() { Page = Trace.File };

                Pages.Add(page);
            }

            foreach (var superglobal in PhpName.Superglobals)
            {
                var fields = page.SuperglobalNameCollectionTable[superglobal];
                var traceFields = Trace.Calls
                    .Where(x => x.ParameterValues.Any() && x.Name == superglobal && !fields.Contains(x.ParameterValues[0]))
                    .Select(x => x.ParameterValues[0]);

                if (!traceFields.Any())
                    continue;

                fields.AddRange(traceFields);
            }
        }

        public string ToXml()
        {
            var serializer = new XmlSerializer(typeof(ApplicationMap));
            StringWriter writer = new StringWriter();

            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, this);
                stream.Position = 0;

                using (var reader = new StreamReader(stream))
                    return reader.ReadToEnd();
            }
        }

        public static ApplicationMap FromXml(string Filename)
        {
            var serializer = new XmlSerializer(typeof(ApplicationMap));
            return serializer.Deserialize(Filename) as ApplicationMap;
        }
    }
}
