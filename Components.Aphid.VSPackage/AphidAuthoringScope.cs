using Microsoft.VisualStudio.Package;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Aphid.VSPackage
{
    public class AphidDeclaration
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string DisplayText { get; set; }

        public AphidDeclaration(string name, string description, string displayText)
        {
            Name = name;
            Description = description;
            DisplayText = displayText;
        }
    }

    public class AphidDeclarations : Declarations
    {
        private List<AphidDeclaration> _declarations = new List<AphidDeclaration>()
        {
            new AphidDeclaration("test", "a test", "testDisplay")
        };

        public override int GetCount()
        {
            return _declarations.Count;
        }

        public override string GetDescription(int index)
        {
            return _declarations[index].Description;
        }

        public override string GetDisplayText(int index)
        {
            return _declarations[index].DisplayText;
        }

        public override int GetGlyph(int index)
        {
           return 0;
        }

        public override string GetName(int index)
        {
            return _declarations[index].Name;
        }
    }

    public class AphidAuthoringScope : AuthoringScope
    {
        public override string GetDataTipText(int line, int col, out Microsoft.VisualStudio.TextManager.Interop.TextSpan span)
        {
            span = new Microsoft.VisualStudio.TextManager.Interop.TextSpan();
            return null;
        }

        public override Declarations GetDeclarations(Microsoft.VisualStudio.TextManager.Interop.IVsTextView view, int line, int col, TokenInfo info, ParseReason reason)
        {
            return new AphidDeclarations();
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
}
