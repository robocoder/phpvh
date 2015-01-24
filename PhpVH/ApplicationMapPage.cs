using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml.Serialization;

namespace PhpVH
{
    [Serializable]
    public class ApplicationMapPage
    {
        [XmlAttribute]
        public string Page { get; set; }

        [XmlElement]
        public List<string> Get { get; set; }

        [XmlElement]
        public List<string> Post { get; set; }

        [XmlElement]
        public List<string> Request { get; set; }

        [XmlElement]
        public List<string> Files { get; set; }

        [XmlElement]
        public List<string> Cookie { get; set; }

        [XmlIgnore]
        public Dictionary<string, List<string>> SuperglobalNameCollectionTable { get; private set; }

        public ApplicationMapPage()
        {
            Get = new List<string>();
            Post = new List<string>();
            Request = new List<string>();
            Files = new List<string>();
            Cookie = new List<string>();

            SuperglobalNameCollectionTable = new Dictionary<string, List<string>>()
            {
                { PhpName.Get, Get },
                { PhpName.Post, Post },
                { PhpName.Request, Request },
                { PhpName.Files, Files },
                { PhpName.Cookie, Cookie },
            };
        }

        public Dictionary<string, List<string>> ToDictionary()
        {
            var properties = this
                .GetType()
                .GetProperties()
                .Where(x => new[] 
                { 
                    "Get", 
                    "Post", 
                    "Request", 
                    "Files", 
                    "Cookie" 
                }
                    .Contains(x.Name))
                .Select(x => new
                {
                    Name = x.Name,
                    Value = x.GetValue(this, null) as List<string>
                })
                .Where(x => x.Value != null)
                .ToDictionary(x => x.Name, x => x.Value);

            return properties;
        }
    }
}
