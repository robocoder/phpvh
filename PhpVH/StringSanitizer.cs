using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhpVH
{
    public static class StringSanitizer
    {
        public unsafe static string RemoveBeeps(string text)
        {
            fixed (char* t = text)
            {
                for (int x = 0; x < text.Length; x++)
                    if (t[x] == (char)7)
                        t[x] = ' ';

                return text;
            }            
        }
    }
}
