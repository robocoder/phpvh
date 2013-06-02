using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components
{
    public static class CodeMemberMethodExtension
    {
        public static void AddParameter(this CodeMemberMethod method, Type type, string name)
        {
            method.Parameters.Add(new CodeParameterDeclarationExpression(type, name));
        }

        public static void AddParameter(this CodeMemberMethod method, string type, string name)
        {
            method.Parameters.Add(new CodeParameterDeclarationExpression(type, name));
        }
    }
}
