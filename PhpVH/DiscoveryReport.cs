using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhpVH.ScanPlugins;
using Components;

namespace PhpVH
{
    public static class DiscoveryReport
    {
        public static string Create(TraceTable traceTable)
        {
            var discoveryReport = new StringBuilder();
            foreach (var scan in traceTable)
            {
                discoveryReport.AppendLine(new string('-', 64));
                discoveryReport.AppendLine();
                discoveryReport.AppendLine(scan.Key.ToString());
                discoveryReport.AppendLine();
                discoveryReport.AppendLine(new string('-', 64));

                foreach (var mode in scan.Value)
                {
                    foreach (var targetFile in mode.Value)
                    {
                        discoveryReport.AppendLine();
                        discoveryReport.Append(targetFile.Key);
                        discoveryReport.AppendLine();
                        discoveryReport.AppendLine();
                        discoveryReport.Append(targetFile.Value.ToString().Indent());
                        discoveryReport.AppendLine();
                    }
                }
            }
            
            return discoveryReport.ToString();
        }
    }
}
