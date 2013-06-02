using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhpVH.LexicalAnalysis;
using Components;

namespace PhpVH.StaticAnalysis
{
    public abstract class StaticAnalysisPlugin
    {
        protected abstract string Name { get; }

        public abstract StaticAnalysisAlert[] GetAlerts(string code, PhpToken[] tokens);

        protected string GetSurroundingCode(string code, PhpToken token)
        {
            var line = code.GetLineNumber(token.Index);
            var code2 = code.InsertLineNumbers();
            var index = code2.GetLineIndex(line);

            return code2.GetSurroundingLines(index, 9, 9);
        }

        protected StaticAnalysisAlert CreateAlert(string code, PhpToken token)
        {
            return new StaticAnalysisAlert(Name,
                code.GetLineNumber(token.Index) + 1, 
                GetSurroundingCode(code, token));
        }
    }
}
