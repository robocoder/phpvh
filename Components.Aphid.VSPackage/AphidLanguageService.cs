using Components.Aphid.Interpreter;
using Components.Aphid.Lexer;
using Components.Aphid.Parser;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Aphid.VSPackage
{


    public class AphidLanguageService : LanguageService
    {
        public override string GetFormatFilterList()
        {
            return "Aphid files (*.alx)\n*.alx\n";
        }

        public override LanguagePreferences GetLanguagePreferences()
        {
            return new LanguagePreferences()
            {
                EnableCodeSense = true,
                CodeSenseDelay = 500,
                AutoListMembers = true,
                EnableFormatSelection = true,
                EnableMatchBraces = true,                
                LineNumbers = true,
                EnableCommenting = true,
                EnableShowMatchingBrace = true,
                IndentStyle = IndentingStyle.Smart,
                EnableQuickInfo = true,
                MaxErrorMessages = 100,

            };
        }

        public override IScanner GetScanner(Microsoft.VisualStudio.TextManager.Interop.IVsTextLines buffer)
        {
            return new AphidScanner();
        }

        public override string Name
        {
            get { return "Aphid Language Service"; }
        }

        private void CheckParseRequest(ParseRequest req)
        {
            try
            {
                var lexer = new AphidLexer(req.Text);
                var parser = new AphidParser(lexer.GetTokens());
                parser.Parse();
            }
            catch (AphidParserException e)
            {
                var lineCol = TokenHelper.GetLineCol(req.Text, e.Token.Index);
                var span = new TextSpan()
                {
                    iStartLine = lineCol.Item1,
                    iEndLine = lineCol.Item1,
                    iStartIndex = lineCol.Item2,
                    iEndIndex = lineCol.Item2 + (e.Token.Lexeme != null ? e.Token.Lexeme.Length : 0)
                };

                var msg = string.Format("Unexpected {0}: {1}", e.Token.TokenType.ToString(), e.Token.Lexeme);

                req.Sink.AddError(req.FileName, msg, span, Severity.Error);
            }
        }

        public override AuthoringScope ParseSource(ParseRequest req)
        {
            switch (req.Reason)
            {
                case ParseReason.MatchBraces:
                    break;

                case ParseReason.HighlightBraces:
                    break;

                case ParseReason.Check:
                    CheckParseRequest(req);
                    break;

                default:
                    break;
            }

            return new AphidAuthoringScope();
        }
    }
}
