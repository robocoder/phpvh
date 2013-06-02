using Components.Aphid.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhpVH.SelfTest
{
    public class SelfTest
    {
        [AphidProperty("name")]
        public string Name { get; set; }

        [AphidProperty("arg")]
        public string Arg { get; set; }

        [AphidProperty("msgs")]
        public string[] Messages { get; set; }

        [AphidProperty("required")]
        public string[] Required { get; set; }

        public SelfTest()
        {
        }

        public SelfTest(string name, string arg, string[] messages)
        {
            Name = name;
            Arg = arg;
            Messages = messages;
        }
    }
}
