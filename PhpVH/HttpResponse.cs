using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhpVH
{
    public class HttpResponse
    {
        public string Header { get; set; }

        public string Body { get; set; }

        public string CompleteResponse
        {
            get { return Header + "\r\n\r\n" + Body; }
        }

        public void ParseChunkedBody(string RawBody)
        {
            Body = "";

            if (RawBody == "0\r\n\r\n")
                return;

            var index = RawBody.IndexOf("\r\n");

            int chunkLength = 0;
            while ((chunkLength = int.Parse(RawBody.Remove(index), System.Globalization.NumberStyles.HexNumber)) != 0)
            {
                Body += RawBody.Substring(index + 2, chunkLength);
                RawBody = RawBody.Substring(index + 4 + chunkLength);

                index = RawBody.IndexOf("\r\n");
            }
        }
    }
}
