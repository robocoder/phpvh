using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using PhpVH.CodeAnalysis;

namespace PhpVH
{
    public class RequestBuilder
    {
        private IEnumerable<TracedFunctionCall> _calls;

        public IEnumerable<TracedFunctionCall> Calls
        {
            get { return _calls; }
            set { _calls = value; }
        }

        public RequestBuilder()
        {
            _calls = new TracedFunctionCall[0];
        }

        public RequestBuilder(IEnumerable<TracedFunctionCall> calls)
        {
            _calls = calls;
        }

        public string CreateRequest(string Filename, string Server, string BadChars, bool Get, bool Anchor, bool FuzzCookies)
        {
            var anchorId = 0;

            Func<string> getAnchor = () => Anchor ? anchorId++.ToString() : "";

            string _queryString = "";

            var getFields = new List<string>();

            foreach (TracedFunctionCall c in _calls.Where(x =>
                (x.Name == "$_GET" || x.Name == "$_REQUEST")))
            {
                if (!c.ParameterValues.Any())
                    c.ParameterValues.Add("x");

                var key = c.ParameterValues[0];
                if (getFields.Contains(key))
                    continue;

                getFields.Add(key);

                _queryString += (_queryString.Length != 0 ? "&" : "?") +
                    key + "=" + getAnchor() + HttpUtility.UrlEncode(BadChars);
            }

            var content = "";

            if (!Get)
            {
                var postFields = new List<string>();

                foreach (TracedFunctionCall c in _calls.Where(x => x.Name == "$_POST"))
                {
                    if (!c.ParameterValues.Any())
                        c.ParameterValues.Add("x");

                    if (postFields.Contains(c.ParameterValues[0]))
                        continue;

                    postFields.Add(c.ParameterValues[0]);

                    content +=
                        "------x\r\n" +
                        "Content-Disposition: form-data; name=\"" + c.ParameterValues[0] + "\"\r\n" +
                        "\r\n" +
                        getAnchor() + BadChars + "\r\n";
                }

                if (content.Length > 0)
                    content +=
                        "------x--\r\n" +
                        "\r\n";
            }

            string method = Get ? "GET" : "POST";
            string contentType = Get ?
                "" : "Content-Type: multipart/form-data; boundary=----x\r\n";

            var cookieString = new StringBuilder();

            if (FuzzCookies)
            {
                var cookieFields = new List<string>();

                foreach (var c in _calls.Where(x => x.Name == "$_COOKIE"))
                {
                    if (!c.ParameterValues.Any())
                        c.ParameterValues.Add("x");

                    if (cookieFields.Contains(c.ParameterValues[0]))
                        continue;

                    cookieFields.Add(c.ParameterValues[0]);

                    cookieString.Append((cookieString.Length != 0 ? "; " : "") +
                        c.ParameterValues.First() + "=" + getAnchor() +
                        HttpUtility.UrlEncode(BadChars));
                }

                if (cookieString.Length != 0)
                    cookieString.Insert(0, "Cookie: ").Append("\r\n");
            }

            var header =
                method + " " + Filename + _queryString + " HTTP/1.1\r\n" +
                "Host: " + Server + "\r\n" +
                "Proxy-Connection: keep-alive\r\n" +
                "User-Agent: x\r\n" +
                "Content-Length: " + content.Length + "\r\n" +
                "Cache-Control: max-age=0\r\n" +
                "Origin: null\r\n" +
                contentType +
                cookieString +
                "Accept: text/html\r\n" +
                //"Accept-Encoding: gzip,deflate,sdch\r\n" +
                "Accept-Language: en-US,en;q=0.8\r\n" +
                "Accept-Charset: ISO-8859-1,utf-8;q=0.7,*;q=0.3\r\n" +
                "\r\n";

            return header + content;
        }
    }
}
