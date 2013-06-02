using Components.Aphid.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReleasePackager
{
    public class Cleanup
    {
        [AphidProperty("dirs")]
        public string[] Directories { get; set; }

        [AphidProperty("files")]
        public string[] Files { get; set; }

        [AphidProperty("fileRegexes")]
        public string[] FilePatterns { get; set; }
    }
}
