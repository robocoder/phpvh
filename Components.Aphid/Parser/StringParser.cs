using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components.Aphid.Parser
{
    public static class StringParser
    {
        public static string Parse(string value)
        {
            var delim = value[0];
            var s = value.Substring(1);
            s = s.Remove(s.Length - 1);
            var sb = new StringBuilder();

            var state = 0;

            for (int i = 0; i < s.Length; i++)
            {
                var c = s[i];

                switch (state)
                {
                    case 0:
                        switch (c)
                        {
                            case '\\':
                                state = 1;
                                break;

                            default:
                                sb.Append(c);
                                break;
                        }
                        break;

                    case 1:
                        switch (c)
                        {
                            case '\\':
                                sb.Append('\\');
                                state = 0;
                                break;

                            case 'r':
                                sb.Append('\r');
                                state = 0;
                                break;

                            case 'n':
                                sb.Append('\n');
                                state = 0;
                                break;

                            case '"':
                            case '\'':
                                if (c != delim)
                                {
                                    throw new InvalidOperationException();
                                }

                                sb.Append(delim);
                                state = 0;
                                break;

                            default:
                                throw new InvalidOperationException("Invalid escape sequence");
                        }
                        break;
                }
            }

            return sb.ToString();
        }
    }
}
