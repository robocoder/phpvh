using Components.Aphid.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhpVH.SelfTest
{
    public class SelfTestAssembly
    {
        [AphidProperty("asm")]
        public string Assembly { get; set; }

        [AphidProperty("tests")]
        public SelfTest[] Tests { get; set; }
    }
}
