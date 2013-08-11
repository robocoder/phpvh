using Components.Aphid.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Aphid.Interpreter
{
    public class AphidFunction
    {
        public string[] Args { get; set; }

        public List<Expression> Body { get; set; }

        public AphidObject ParentScope { get; set; }
    }
}
