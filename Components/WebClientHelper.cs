using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    public static class WebClientHelper
    {
        public static WebClientHelperResponse DownloadData(string Address)
        {
            var info = "";
            byte[] data = new byte[0];

            try
            {
                var req = HttpWebRequest.Create(Address) as HttpWebRequest;
                req.AllowAutoRedirect = false;
                req.UserAgent = "Mozilla/5.0 (Windows NT 6.0) AppleWebKit/537.1 (KHTML, like Gecko) Chrome/21.0.1180.57 Safari/537.1";

                var resp = (HttpWebResponse)req.GetResponse();

                info = ((int)resp.StatusCode).ToString();

                using (var stream = resp.GetResponseStream())
                {
                    var bufferSize = 8192;
                    var buffer = new byte[bufferSize];
                    var bytesRead = 0;
                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        Array.Resize(ref buffer, bytesRead);
                        var oldLength = data.Length;
                        Array.Resize(ref data, data.Length + bytesRead);
                        Array.Copy(buffer, 0, data, oldLength, buffer.Length);
                        buffer = new byte[bufferSize];
                    }
                }
            }
            catch (WebException e)
            {

                info = e.Status.ToString();

                var resp = (e.Response as HttpWebResponse);

                if (resp != null)
                {
                    info = ((int)resp.StatusCode).ToString();

                    using (var stream = resp.GetResponseStream())
                    {
                        var bufferSize = 8192;
                        var buffer = new byte[bufferSize];
                        var bytesRead = 0;
                        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            Array.Resize(ref buffer, bytesRead);
                            var oldLength = data.Length;
                            Array.Resize(ref data, data.Length + bytesRead);
                            Array.Copy(buffer, 0, data, oldLength, buffer.Length);
                            buffer = new byte[bufferSize];
                        }
                    }
                }
            }

            return new WebClientHelperResponse(info, data);

            //var client = new System.Net.WebClient();
            //return client.DownloadData(Address);
        }
    }
}
