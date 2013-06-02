using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Components;

namespace PhpVH
{
    public class BodyChunk
    {
        public string Body { get; set; }
        public bool LastChunk { get; set; }

        public BodyChunk(string Body, bool LastChunk)
        {
            this.Body = Body;
            this.LastChunk = LastChunk;
        }

        public static BodyChunk ReadChunk(string RawBody, Stream ResponseStream)
        {
            if (RawBody == "0\r\n\r\n")
                return new BodyChunk("", true);

            var bodyIndex = RawBody.IndexOf("\r\n");

            var chunkLength = int.Parse(RawBody.Remove(bodyIndex), System.Globalization.NumberStyles.HexNumber);

            var bodyLength = RawBody.Length - bodyIndex + 2;

            while (bodyLength <= chunkLength)
            {
                var resp = ResponseStream.ReadString(8192 * 4);

                bodyLength += resp.Length;

                RawBody += resp;
            }

            var body = RawBody.Substring(bodyIndex + 2, chunkLength);

            var end = bodyIndex + 2 + chunkLength;

            var tail = RawBody.Substring(end);

            return new BodyChunk(body, tail.Contains("0"));
        }
    }
}
