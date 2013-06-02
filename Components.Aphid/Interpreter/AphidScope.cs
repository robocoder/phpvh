using System;

namespace Components.Aphid.Interpreter
{
    public class AphidScope
    {
        public AphidScope Parent { get; set; }

        public AphidObject Variables { get; set; }

        public AphidScope ()
        {
        }

        public AphidScope(AphidObject variables)
        {
            Variables = variables;
        }

        public AphidScope(AphidObject variables, AphidScope parent)
            : this(variables)
        {
            Parent = parent;
        }
    }
}

