using System.Collections.Generic;
using NUnit.Framework;
using PhpVH.LexicalAnalysis;

namespace PhpVH.Tests.Unit.LexicalAnalysis
{
    public static class LexerAssert
    {
        public static void IsEmpty(PhpLexer lexer)
        {
            CollectionAssert.AreEqual(new List<PhpToken>(), lexer.GetTokens());
        }
    }
}
