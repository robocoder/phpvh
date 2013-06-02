using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components
{
    public static class CodeMethodReferenceExpressionExtension
    {
        public static CodeMethodInvokeExpression Invoke(this CodeMethodReferenceExpression expression, params CodeExpression[] parameters)
        {
            return new CodeMethodInvokeExpression(expression, parameters);
        }

        
    }
}
