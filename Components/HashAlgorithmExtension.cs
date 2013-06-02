using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Components
{
    public static class HashAlgorithmExtension
    {
        public static string ComputeHash(this HashAlgorithm Hash, string Buffer)
        {
            StringBuilder digestString = new StringBuilder();

            foreach (byte b in Hash.ComputeHash(ASCIIEncoding.ASCII.GetBytes(Buffer)))
                digestString.Append(Convert.ToString(b, 16).PadLeft(2, '0'));

            return digestString.ToString();
        }
    }
}
