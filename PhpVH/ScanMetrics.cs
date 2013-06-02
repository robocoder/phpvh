using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using PhpVH.LexicalAnalysis;
using PhpVH.CodeCoverage;
using System.Web;
using Components;

namespace PhpVH
{
    public class ScanMetrics
    {
        public static ScanMetrics Default = new ScanMetrics();

        public Annotator Annotator { get; set; }

        public PluginAnnotationTable PluginAnnotations { get; set; }

        public ScanMetrics()
        {
            PluginAnnotations = new PluginAnnotationTable();
            Annotator = new Annotator();
        }
    }
}
