using Components.Aphid.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components.Aphid.Interpreter
{
    public class OperatorHelper
    {
        private Func<AphidObject, AphidObject> _callInitFunction;

        public OperatorHelper(Func<AphidObject, AphidObject> callInitFunction)
        {
            _callInitFunction = callInitFunction;
        }

        public AphidObject Add(AphidObject x, AphidObject y)
        {
            if (x.Value is decimal && y.Value is decimal)
            {
                return new AphidObject((decimal)x.Value + (decimal)y.Value);
            }            
            else
            {
                return _callInitFunction(new AphidObject(Convert.ToString(x.Value) + Convert.ToString(y.Value)));
            }
        }

        public AphidObject Mod(AphidObject x, AphidObject y)
        {
            return new AphidObject((decimal)x.Value % (decimal)y.Value);
        }

        public AphidObject BinaryOr(AphidObject x, AphidObject y)
        {
            return new AphidObject((decimal)((long)(decimal)x.Value | (long)(decimal)y.Value));
        }

        public AphidObject BinaryAnd(AphidObject x, AphidObject y)
        {
            return new AphidObject((decimal)((long)(decimal)x.Value & (long)(decimal)y.Value));
        }

        public AphidObject BinaryShiftLeft(AphidObject x, AphidObject y)
        {
            return new AphidObject((decimal)((int)(decimal)x.Value << (int)(decimal)y.Value));
        }

        public AphidObject BinaryShiftRight(AphidObject x, AphidObject y)
        {
            return new AphidObject((decimal)((int)(decimal)x.Value << (int)(decimal)y.Value));
        }

        public AphidObject Xor(AphidObject x, AphidObject y)
        {
            return new AphidObject((decimal)((int)(decimal)x.Value ^ (int)(decimal)y.Value));
        }

        public AphidObject Subtract(AphidObject x, AphidObject y)
        {
            return new AphidObject(((decimal)x.Value) - (decimal)y.Value);
        }

        public AphidObject Multiply(AphidObject x, AphidObject y)
        {
            object val;
            if (x.Value is Decimal)
            {
                val = ((decimal)x.Value) * (decimal)y.Value;
            }
            else if (x.Value is string)
            {
                var s = x.Value as string;
                var sb = new StringBuilder();

                for (int i = 0; i < (decimal)y.Value; i++)
                {
                    sb.Append(s);
                }

                return _callInitFunction(new AphidObject(sb.ToString()));

                //val = sb.ToString();
            }
            else
            {
                throw new AphidRuntimeException("Could not multiply type");
            }

            return new AphidObject(val);
        }

        public AphidObject Divide(AphidObject x, AphidObject y)
        {
            return new AphidObject(((decimal)x.Value) / (decimal)y.Value);
        }
    }
}
