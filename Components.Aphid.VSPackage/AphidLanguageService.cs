using Microsoft.VisualStudio.Package;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Aphid.VSPackage
{
    public class AphidAuthoringScope : AuthoringScope
    {
        public override string GetDataTipText(int line, int col, out Microsoft.VisualStudio.TextManager.Interop.TextSpan span)
        {
            span = new Microsoft.VisualStudio.TextManager.Interop.TextSpan();
            return null;
        }

        public override Declarations GetDeclarations(Microsoft.VisualStudio.TextManager.Interop.IVsTextView view, int line, int col, TokenInfo info, ParseReason reason)
        {
            return null;
        }

        public override Methods GetMethods(int line, int col, string name)
        {
            return null;
        }

        public override string Goto(Microsoft.VisualStudio.VSConstants.VSStd97CmdID cmd, Microsoft.VisualStudio.TextManager.Interop.IVsTextView textView, int line, int col, out Microsoft.VisualStudio.TextManager.Interop.TextSpan span)
        {
            span = new Microsoft.VisualStudio.TextManager.Interop.TextSpan();

            return null;
        }
    }


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
                //EnableCodeSense = true,
                //AutoListMembers = true,
                EnableFormatSelection = true,
                //EnableMatchBraces = true,
                //IndentSize = 4,
                LineNumbers = true,
                EnableCommenting = true,
                //EnableShowMatchingBrace = true,
                IndentStyle = IndentingStyle.Smart,
            };
        }

        public override IScanner GetScanner(Microsoft.VisualStudio.TextManager.Interop.IVsTextLines buffer)
        {
            return new AphidScanner();
        }

        public override string Name
        {
            get { 
                return "Aphid Language Service"; }
        }

        public override AuthoringScope ParseSource(ParseRequest req)
        {
            switch (req.Reason)
            {
                case ParseReason.MatchBraces:
                    break;

                case ParseReason.HighlightBraces:
                    
                    break;

                default:
                    break;
            }

            return new AphidAuthoringScope();
        }
    }
}
