using Components;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AphidCodeGenerator
{
    class AphidLexerGenerator : IAphidCodeObject
    {
        public string CodeFile
        {
            get { { return @"Lexer\AphidLexer.cs"; } }
        }

        public System.CodeDom.CodeObject CreateCodeObject()
        {
            var llex = PathHelper.GetExecutingPath("llex.exe");
            var tmpFile = Path.GetTempFileName();
            
            AppDomain.CurrentDomain.ExecuteAssembly(
                llex,
                new[]
                {
                    "Aphid.alx",
                    tmpFile
                });

            var snippet = new CodeSnippetCompileUnit(File.ReadAllText(tmpFile));
            File.Delete(tmpFile);
            return snippet;
        }
    }
}
