using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Aphid.VSPackage
{
    public static class TokenHelper
    {
        private enum LineState
        {
            RegularChar,
            ReturnCarriage,
        }

        public static Tuple<int, int> GetLineCol(string text, int index)
        {
            var state = LineState.RegularChar;
            var preceding = text.Remove(index);
            var line = 0;
            var col = 0;
            for (int i = 0; i < preceding.Length; i++)
            {
                var c = preceding[i];
                switch (state)
                {
                    case LineState.RegularChar:
                        switch (c)
                        {
                            case '\r':
                                line++;
                                col = 0;
                                state = LineState.ReturnCarriage;
                                break;
                            case '\n':
                                line++;
                                col = 0;
                                break;
                            default:
                                col++;
                                break;
                        }
                        break;

                    case LineState.ReturnCarriage:
                        switch (c)
                        {
                            case '\r':
                                line++;
                                col = 0;
                                break;
                            case '\n':
                                col = 0;
                                state = LineState.RegularChar;
                                break;
                            default:
                                col++;
                                state = LineState.RegularChar;
                                break;
                        }
                        break;
                }
            }

            return Tuple.Create(line, col);
        }
    }
}
