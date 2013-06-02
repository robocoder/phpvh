using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using Components;

namespace PhpVH
{
    [XmlType("ArrayOfScanAlert")]
    public class ScanAlertCollection : List<ScanAlert>
    {
        private static XmlSerializer _serializer = new XmlSerializer(typeof(ScanAlertCollection));

        public static ScanAlertCollection Load(string Filename)
        {
            var s = _serializer.Deserialize(Filename) as ScanAlertCollection;
            s.UnescapeValues();
            return s;
        }

        public void Save(string Filename)
        {
            EscapeValues();

            _serializer.Serialize(Filename, this);

            UnescapeValues();
        }

        public string ToXml()
        {
            EscapeValues();
            return _serializer.Serialize(this);
        }

        private string Escape(string Value)
        {
            if (Value == null)
                return Value;

            Value = Regex.Escape(Value);

            for (int i = Value.Length - 1; i >= 0; i--)
            {
                if (Value[i] < 0x20 || Value[i] > 0x7F)
                {
                    Value = Value
                        .Remove(i, 1)
                        .Insert(i, "\\x" + Convert.ToString(Value[i], 16).PadLeft(2, '0'));
                }
            }

            return Value;
        }

        private string Unescape(string Value)
        {
            return Value != null ? Regex.Unescape(Value) : Value;
        }

        private void TransformValues(Func<string, string> Transform)
        {
            foreach (var a in this)
            {
                a.Trace.Request = Transform(a.Trace.Request);
                a.Trace.Response = Transform(a.Trace.Response);
                a.Trace.RawText = Transform(a.Trace.RawText);

                foreach (var c in a.Trace.Calls)
                {
                    c.Value = Transform(c.Value);

                    for (int i = 0; i < c.ParameterValues.Count; i++)
                        c.ParameterValues[i] = Transform(c.ParameterValues[i]);
                }
            }
        }

        private void EscapeValues()
        {
            TransformValues(Escape);
        }

        private void UnescapeValues()
        {
            TransformValues(Unescape);
        }
    }
}
