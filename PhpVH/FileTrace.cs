using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Components;

namespace PhpVH
{
    [Serializable]
    public class FileTrace
    {
        public const string TraceStart = "--------------------------------Start",
            TraceEnd = "--------------------------------End";

        private string _file;
        
        [XmlAttribute]
        public string File
        {
            get { return _file; }
            set { _file = value; }
        }

        private string _request;

        [XmlElement]
        public string Request
        {
            get { return _request; }
            set { _request = value; }
        }

        private string _response;

        [XmlElement]
        public string Response
        {
            get { return _response; }
            set { _response = value; }
        }

        private string _rawText;

        [XmlElement]
        public string RawText
        {
            get { return _rawText; }
            set { _rawText = value; }
        }

        private List<TracedFunctionCall> _calls = new List<TracedFunctionCall>();

        [XmlElement]
        public List<TracedFunctionCall> Calls
        {
            get { return _calls; }
            set { _calls = value; }
        }

        public static string TruncateValue(string Value)
        {
            return Value;
        }

        private static string ParseNextFunction(StreamReader Reader)
        {
            var text = new StringBuilder();

            var start = Reader.ReadLine();

            if (start == null)
                return null;
            else if (start != TraceStart)
                throw new InvalidOperationException("Unknown token:\r\n"
                    + start);

            string line = "";

            while ((line = Reader.ReadLine()) != TraceEnd)
            {
                if (line == null)
                    throw new InvalidOperationException("No termination token.");

                text.Append(TruncateValue(line) + "\n");
            }

            return text.ToString();
        }       

        public static FileTrace Parse(StreamReader Reader)
        {
            var trace = new FileTrace();
            var Text = Reader.ReadToEnd();
            trace.RawText = Text;

            var functions = Text.Split(new string[]
            {
                "--------------------------------Start\n",
                "--------------------------------End\n",
            }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var f in functions)
            {
                var call = new TracedFunctionCall()
                {
                    Name = Regex.Match(f, @"Function Called:\s(.+)\n").Groups[1].Value
                };

                var matches = Regex.Matches(f, @"\$Param\d+:\s(((?!(\$Param\d+:\s)|(Value:\s)|(-{32,}End))[\s\S])+)\n",
                    RegexOptions.Multiline);

                foreach (Match m in matches)
                    call.ParameterValues.Add(m.Groups[1].Value);

                Match valueMatch = Regex.Match(f, @"Value:\s(.+)\n");

                if (valueMatch.Success)
                    call.Value = valueMatch.Groups[1].Value;

                trace.Calls.Add(call);                
            }

            return trace;
        }

        public override string ToString()
        {
            var s = new StringBuilder();

            var callDictionary = new Dictionary<string,Dictionary<string, string>>();
            
            foreach (var call in _calls)
            {
                if (!callDictionary.ContainsKey(call.Name))
                    callDictionary.Add(call.Name, new Dictionary<string, string>());

                if (call.ParameterValues.Any() &&
                    !callDictionary[call.Name].ContainsKey(call.ParameterValues[0]))
                    callDictionary[call.Name].Add(call.ParameterValues[0], call.Value);
            }

            foreach (var call in callDictionary)
            {
                s.Append(call.Key);
                s.AppendLine();

                foreach (var callParam in call.Value)
                {
                    s.Append(callParam.Key.Indent() + " = " + callParam.Value);
                    s.AppendLine();
                }

                s.AppendLine();
            }

            return s.ToString();
        }
    }
}
