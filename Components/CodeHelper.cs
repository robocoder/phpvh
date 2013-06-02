using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components
{
    public static class CodeHelper
    {
        public static CodeVariableReferenceExpression VarRef(string name)
        {
            return new CodeVariableReferenceExpression(name);
        }

        public static CodeVariableDeclarationStatement VarDecl(Type type, string name)
        {
            return new CodeVariableDeclarationStatement(type, name);
        }

        public static CodeVariableDeclarationStatement VarDecl(Type type, string name, CodeExpression init)
        {
            return new CodeVariableDeclarationStatement(type, name, init);
        }

        public static CodeVariableDeclarationStatement VarDecl(string type, string name)
        {
            return new CodeVariableDeclarationStatement(type, name);
        }

        public static CodeVariableDeclarationStatement VarDecl(string type, string name, CodeExpression init)
        {
            return new CodeVariableDeclarationStatement(type, name, init);
        }

        public static CodeVariableDeclarationStatement VarDecl(string name)
        {
            return new CodeVariableDeclarationStatement("var", name);
        }

        public static CodeVariableDeclarationStatement VarDecl(string name, CodeExpression init)
        {
            return new CodeVariableDeclarationStatement("var", name, init);
        }

        public static CodePropertyReferenceExpression PropRef(CodeExpression targetObject, string propertyName)
        {
            return new CodePropertyReferenceExpression(targetObject, propertyName);
        }

        public static CodePropertyReferenceExpression PropRef(string propertyName)
        {
            return PropRef(This(), propertyName);
        }

        public static CodeBinaryOperatorExpression BinOpExp(CodeExpression left, CodeBinaryOperatorType op, CodeExpression right)
        {
            return new CodeBinaryOperatorExpression(left, op, right);
        }

        public static CodeExpression BinOpExpJoin(this IEnumerable<CodeExpression> expressions, CodeBinaryOperatorType op)
        {
            CodeExpression codeExp = expressions.First();

            foreach (var x in expressions.Skip(1))
            {
                codeExp = CodeHelper.BinOpExp(codeExp, op, x);
            }

            return codeExp;
        }

        public static CodeArgumentReferenceExpression Arg(string name)
        {
            return new CodeArgumentReferenceExpression(name);
        }

        public static CodeTypeReference TypeRef(Type type)
        {
            return new CodeTypeReference(type);
        }

        public static CodeTypeReference TypeRef<T>()
        {
            return TypeRef(typeof(T));
        }

        public static CodeTypeReferenceExpression TypeRefExp(Type type)
        {
            return new CodeTypeReferenceExpression(type);
        }

        public static CodeTypeReferenceExpression TypeRefExp<T>()
        {
            return TypeRefExp(typeof(T));
        }

        public static CodeTypeOfExpression TypeOf(string type)
        {
            return new CodeTypeOfExpression(type);
        }

        public static CodeTypeOfExpression TypeOf(Type type)
        {
            return new CodeTypeOfExpression(type);
        }

        public static CodeMethodReturnStatement Return(CodeExpression expression)
        {
            return new CodeMethodReturnStatement(expression);
        }

        public static CodePrimitiveExpression True()
        {
            return new CodePrimitiveExpression(true);
        }

        public static CodePrimitiveExpression False()
        {
            return new CodePrimitiveExpression(false);
        }

        public static CodePrimitiveExpression Null()
        {
            return new CodePrimitiveExpression();
        }

        public static CodeThisReferenceExpression This()
        {
            return new CodeThisReferenceExpression();
        }

        public static CodeObjectCreateExpression New(Type createType, params CodeExpression[] parameters)
        {
            return new CodeObjectCreateExpression(createType, parameters);
        }

        public static CodeAssignStatement Assign(CodeExpression left, CodeExpression right)
        {
            return new CodeAssignStatement(left, right);
        }

        public static CodeIterationStatement While(CodeExpression testExpression)
        {
            return new CodeIterationStatement(
                new CodeSnippetStatement(""),
                testExpression,
                new CodeSnippetStatement(""));
        }
    }
}
