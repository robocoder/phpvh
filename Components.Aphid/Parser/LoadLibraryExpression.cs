using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components.Aphid.Parser
{
    public class LoadLibraryExpression : Expression
    {
        public Expression LibraryExpression { get; set; }

        public LoadLibraryExpression(Expression libraryExpression)
        {
            LibraryExpression = libraryExpression;
        }
    }
}
