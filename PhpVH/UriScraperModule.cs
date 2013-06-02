using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace PhpVH
{
    public class UriScraperModule
    {
        public Regex Regex { get; set; }

        public Func<Match, string> ProcessCore { get; set; }

        public UriScraperModule(string pattern, Func<Match, string> processCore)
        {
            Regex = new Regex(
                pattern,
                RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
            ProcessCore = processCore;
        }

        public IEnumerable<string> Process(string html)
        {
            var matches = Regex
                .Matches(html)
                .OfType<Match>();

            return matches.Select(ProcessCore);
        }
    }
}
