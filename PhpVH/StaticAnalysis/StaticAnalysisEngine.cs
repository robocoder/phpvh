using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhpVH.LexicalAnalysis;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using Components;

namespace PhpVH.StaticAnalysis
{
    public class StaticAnalysisEngine
    {
        private static PhpLexer _lexer = new PhpLexer();

        private static StaticAnalysisPlugin[] _plugins = new StaticAnalysisPlugin[]
        {
            new SqlInjectionPlugin(),
            new CommandInjectionPlugin(),
            new CommandInjectionPlugin2(),
            new PhpInjectionPlugin(),
            //new ArbitraryUploadPlugin(),
            new LfiPlugin(),
            new ExtractPlugin(),
            new OpenRedirectPlugin(),
            new XssScanPlugin(),
        };

        private ScanConfig _config;

        public event EventHandler<ItemEventArgs<StaticAnalysisFileAlerts>> FileScanned;

        public StaticAnalysisEngine(ScanConfig config)
        {
            _config = config;
        }

        public IEnumerable<StaticAnalysisAlert> GetAlerts(string code, PhpToken[] tokens)
        {
            return _plugins.SelectMany(x => x.GetAlerts(code, tokens));
        }

        public StaticAnalysisFileAlerts ScanFile(string filename)
        {
            var code = File.ReadAllText(filename).NormalizeLines();

            var alerts = new StaticAnalysisFileAlerts(filename, 
                GetAlerts(code, new PhpLexer(code).GetTokens().ToArray()).ToArray());

            if (FileScanned != null)
                FileScanned(this, new ItemEventArgs<StaticAnalysisFileAlerts>(alerts));

            return alerts;
        }
            
        public StaticAnalysisFileAlertCollection ScanDirectory(string path)
        {
            var dir = new DirectoryInfo(path);

            return new StaticAnalysisFileAlertCollection(dir
                .GetFiles("*.php", SearchOption.AllDirectories)
                .Where(x => x.Extension.ToLower() == ".php")
                .Concat(dir
                    .GetFiles("*.inc", SearchOption.AllDirectories)
                    .Where(x => x.Extension.ToLower() == ".inc"))
                .Select(x => ScanFile(x.FullName))
                .Where(x => x.Alerts.Any())
                .ToArray());
        }
    }
}
