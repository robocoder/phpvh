using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Components;

namespace PhpVH.CodeCoverage
{
    public class CodeCoverageTable : Dictionary<string, decimal>
    {
        public string Plugin { get; set; }

        public decimal Total { get; set; }

        public override string ToString()
        {
            var s = new StringBuilder(Plugin + "\r\n");

            foreach (var k in this
                .Concat(new[] { new KeyValuePair<string, decimal>("Total", Total) })
                .Select(x => string.Format("{0}: {1:0.##}%", x.Key, x.Value)))
                s.AppendLine(k);

            return s.ToString();
        }        
    }
}
