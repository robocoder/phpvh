using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    public class WebClientHelperResponse
    {
        public string Info { get; private set; }

        public byte[] Data { get; private set; }

        public WebClientHelperResponse(string info, byte[] data)
        {
            Info = info;
            Data = data;
        }
    }
}
