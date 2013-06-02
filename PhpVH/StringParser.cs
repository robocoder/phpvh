using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhpVH
{
    public static class StringParser
    {
        public static string[] GetStrings(string Text, bool DoubleQuoteOnly)
        {
            var strings = new List<string>();

            int currentStringIndex = -1;

            bool isString = false;

            char currentDelimiter = 'A';

            bool escaped = false;

            for (int i = 0; i < Text.Length; i++)
            {
                if (!escaped && ((!DoubleQuoteOnly && Text[i] == '\'') || Text[i] == '"'))
                    if (!isString)
                    {
                        isString = true;

                        currentStringIndex = i;
                        currentDelimiter = Text[i];
                    }
                    else if (Text[i] == currentDelimiter)
                    {
                        isString = false;

                        strings.Add(Text.Substring(currentStringIndex, i - currentStringIndex + 1));
                    }

                escaped = !escaped && Text[i] == '\\';
            }

            return strings.ToArray();
        }

        public static string[] GetStrings(string Text)
        {
            return GetStrings(Text, false);
        }
    }
}
