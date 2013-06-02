using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using PhpVH.ScanPlugins;

namespace PhpVH
{
    public class UriScraper
    {
        public Regex Regex { get; set; }

        public UriScraperModule[] Modules { get; set; }
        
        private static Regex _uriRegex = new Regex(
            @"((http(s?)://)|(""(/|(\.\.)))|(((href)|(src))\s*=\s*""))((http(s?)://)?[/a-zA-Z0-9_\-%\.]{4,}):?",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public UriScraper()
        {
            Modules = new[]
            {
                new UriScraperModule(
                    @"(src|href|link)\s*=\s*?(\s|""|')(.*?)(\s|""|')",
                    x => x.Groups[3].Value),
                new UriScraperModule(
                    @"(http(s?)://(.*?))[\s'""<&]",
                    x => x.Groups[1].Value),
            };
        }

        public string[] Parse(string html, string currentUri)
        {
            var uris = Modules
                .SelectMany(x => x.Process(html))
                .Where(x => 
                    (!x.StartsWith("http") || Regex.IsMatch(x)) &&
                    !x.ToLower().StartsWith("mailto:") && 
                    !x.ToLower().StartsWith("javascript:"))
                .Select(x =>
                {
                    try
                    {
                        return (x.StartsWith("https:") || x.StartsWith("http:") ?
                            new Uri(x, UriKind.Absolute) :
                            new Uri(new Uri(currentUri, UriKind.Absolute), x)).ToString();
                    }
                    catch { return null; }
                })
                .Distinct()
                .Where(x => x != null)
                .ToArray();

            return uris;
        }
    }
}
