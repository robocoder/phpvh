using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhpVH.StaticAnalysis
{
    public class StaticAnalysisFileAlerts
    {
        public string Filename { get; set; }
        public StaticAnalysisAlert[] Alerts { get; set; }

        public StaticAnalysisFileAlerts(string filename, StaticAnalysisAlert[] alerts)
        {
            Filename = filename;
            Alerts = alerts;
        }

        public StaticAnalysisFileAlerts()
        {
        }
    }
}
