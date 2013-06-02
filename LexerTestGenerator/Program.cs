using Components;
using Microsoft.CSharp;
using NUnit.Framework;
using PhpVH.LexicalAnalysis;
using PhpVH.Tests.Unit.LexicalAnalysis;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LexerTestGenerator
{
    class Program
    {
        static CodeNamespace CreateNamespace()
        {
            return new CodeNamespace("PhpVH.Tests.Unit.LexicalAnalysis");
        }

        static CodeTypeDeclaration CreateTestClass()
        {
            var testClass = new CodeTypeDeclaration("LexerTest");
            testClass.CustomAttributes.Add(
                new CodeAttributeDeclaration(
                    new CodeTypeReference(
                        typeof(TestFixtureAttribute))));

            testClass.Attributes = MemberAttributes.Public;
            testClass.IsPartial = true;

            return testClass;
        }

        static IEnumerable<string> GetSubstrings(string s)
        {
            return Enumerable.Range(1, s.Length - 1).Select(x => s.Remove(x));
        }

        static IEnumerable<string> GetSuperstrings(string s)
        {
            return Enumerable
                .Range(1, 5)
                .SelectMany(x => new[]
                {
                    s + new string('0', x),
                    s + new string('z', x),
                });
        }

        static IEnumerable<string> GetSubAndSuperstrings(string s)
        {
            return GetSubstrings(s).Concat(GetSuperstrings(s));
        }

        static IEnumerable<CodeMethodInvokeExpression> CreateCalls()
        {
            var keywords = Tokens
                .GetTokens()
                .Where(x => x.Item1.ToString().EndsWith("Keyword"));

            var identifiers = keywords
                .Select(x => x.Item2)
                .SelectMany(GetSubstrings)
                .SelectMany(GetSuperstrings)
                .Distinct()
                .Where(x => !keywords.Any(y => y.Item2 == x))
                .Select(x => Tuple.Create(PhpTokenType.Identifier, x));

            return Tokens
                .GetTokens()
                .Concat(identifiers)
                .Select(x => new CodeMethodInvokeExpression(
                    new CodeTypeReferenceExpression(new CodeTypeReference(typeof(TokenAssert))),
                    "IsValid",
                    new CodeFieldReferenceExpression(
                        new CodeTypeReferenceExpression(typeof(PhpTokenType)),
                        x.Item1.ToString()),
                    new CodePrimitiveExpression(x.Item2)));
        }

        static CodeMemberMethod CreateTestMethod()
        {
            var method = new CodeMemberMethod();
            method.Name = "OperatorAndKeywordTests";
            method.Attributes = MemberAttributes.Public;
            method.CustomAttributes.AddRange(new[]
            {
                new CodeAttributeDeclaration(new CodeTypeReference(typeof(TestAttribute))),
                new CodeAttributeDeclaration(
                    new CodeTypeReference(typeof(CategoryAttribute)),
                    new CodeAttributeArgument(new CodePrimitiveExpression("PhpLexer"))),
            });

            CreateCalls().Iter(x => method.Statements.Add(x));

            return method;
        }

        static void Main(string[] args)
        {
            var compileUnit = new CodeCompileUnit();
            var ns = CreateNamespace();
            compileUnit.Namespaces.Add(ns);

            var testClass = CreateTestClass();
            ns.Types.Add(testClass);

            var method = CreateTestMethod();
            testClass.Members.Add(method);

            var provider = new CSharpCodeProvider();
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            {
                provider.GenerateCodeFromCompileUnit(compileUnit, writer, new CodeGeneratorOptions());
                writer.Flush();
                stream.Position = 0;
                using (var reader = new StreamReader(stream))
                {
                    var code = reader.ReadToEnd();
                    File.WriteAllText(args[0], code);
                    Console.WriteLine(code);
                }
            }
        }
    }
}
