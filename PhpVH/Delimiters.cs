using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhpVH
{
    public struct Delimiters
    {
        public char StartDelimiter, EndDelimiter;

        public Delimiters(DelimiterOptions DelimiterOption)
        {
            switch (DelimiterOption)
            {
                case DelimiterOptions.Parentheses:
                    StartDelimiter = '(';
                    EndDelimiter = ')';
                    break;
                case DelimiterOptions.Brackets:
                    StartDelimiter = '[';
                    EndDelimiter = ']';
                    break;
                case DelimiterOptions.Braces:
                    StartDelimiter = '{';
                    EndDelimiter = '}';
                    break;
                case DelimiterOptions.SingleQuotes:
                    StartDelimiter = '\'';
                    EndDelimiter = '\'';
                    break;
                case DelimiterOptions.DoubleQuotes:
                    StartDelimiter = '"';
                    EndDelimiter = '"';
                    break;
                case DelimiterOptions.None:
                    StartDelimiter = ' ';
                    EndDelimiter = ';';
                    break;
                default:
                    throw new Exception("Invalid Delimiter Option");
            }
        }
    }
}
