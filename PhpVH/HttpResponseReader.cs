using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using Components;

namespace PhpVH
{
    public class HttpResponseReader
    {
        public Stream Stream { get; set; }

        public HttpResponseReader(Stream ResponseStream)
        {
            Stream = ResponseStream;
        }

        public HttpResponse Read()
        {
            var bufferSize = 8192 * 4;
            var response = new HttpResponse();

            var responseString = Stream.ReadString(bufferSize);

            if (responseString.Length == 0)
                return new HttpResponse() { Header = "", Body = "" };

            if (!Regex.IsMatch(responseString, @"HTTP/\d\.\d\s\d+\s"))
                return new HttpResponse()
                {
                    Header = "",
                    Body = responseString
                };

            var bodyIndex = responseString.IndexOf("\r\n\r\n");

            if (bodyIndex == -1)
                return new HttpResponse() 
                {
                    Header = responseString,
                    Body = ""
                };

            response.Header = responseString.Remove(bodyIndex);
            response.Body = "";

            var body = responseString.Substring(bodyIndex + 4);

            if (response.Header.ToLower().Contains("\r\ntransfer-encoding: chunked\r\n"))
            {
                var sb = new StringBuilder(body);

                if (!body.EndsWith("0\r\n\r\n"))
                    while (true)
                    {
                        var c = Stream.ReadString(bufferSize);

                        sb.Append(c);

                        if (c == "" || c.EndsWith("0\r\n\r\n"))
                            break;
                    }

                if (sb.Length > 0)
                {
                    response.ParseChunkedBody(sb.ToString());
                }
                else
                {
                    response.Body = "";
                }
            }
            else
            {
                response.Body = body;
            }

            return response;
        }
    }

}
