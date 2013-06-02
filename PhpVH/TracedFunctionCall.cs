using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Serialization;

namespace PhpVH
{
    public static class FunctionCallExtension
    {
        public static IEnumerable<TracedFunctionCall> Superglobals(this IEnumerable<TracedFunctionCall> Calls)
        {
            return Calls.Where(x => Php.Superglobals.Contains(x.Name));
        }

        
    }

    [Serializable]
    public class TracedFunctionCall
    {
        private string _name;

        [XmlAttribute]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private List<string> _parameterValues = new List<string>();

        [XmlElement]
        public List<string> ParameterValues
        {
            get { return _parameterValues; }
            set { _parameterValues = value; }
        }

        private string _value;

        [XmlElement]
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }
}
