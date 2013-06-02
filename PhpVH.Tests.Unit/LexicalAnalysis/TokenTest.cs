using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PhpVH.LexicalAnalysis;

namespace PhpVH.Tests.Unit.LexicalAnalysis
{
    [TestFixture, Category("PhpLexer")]
    public class TokenTest
    {
        private readonly static List<PhpTokenType> KeywordTokenTypes = new List<PhpTokenType> {
            PhpTokenType.abstractKeyword,
            PhpTokenType.andKeyword,
            PhpTokenType.arrayKeyword,
            PhpTokenType.asKeyword,
            PhpTokenType.breakKeyword,
            PhpTokenType.caseKeyword,
            PhpTokenType.catchKeyword,
            PhpTokenType.cfunctionKeyword,
            PhpTokenType.classKeyword,
            PhpTokenType.cloneKeyword,
            PhpTokenType.constKeyword,
            PhpTokenType.continueKeyword,
            PhpTokenType.declareKeyword,
            PhpTokenType.defaultKeyword,
            PhpTokenType.doKeyword,
            PhpTokenType.elseKeyword,
            PhpTokenType.elseifKeyword,
            PhpTokenType.enddeclareKeyword,
            PhpTokenType.endforKeyword,
            PhpTokenType.endforeachKeyword,
            PhpTokenType.endifKeyword,
            PhpTokenType.endswitchKeyword,
            PhpTokenType.endwhileKeyword,
            PhpTokenType.extendsKeyword,
            PhpTokenType.finalKeyword,
            PhpTokenType.forKeyword,
            PhpTokenType.foreachKeyword,
            PhpTokenType.functionKeyword,
            PhpTokenType.globalKeyword,
            PhpTokenType.gotoKeyword,
            PhpTokenType.ifKeyword,
            PhpTokenType.implementsKeyword,
            PhpTokenType.interfaceKeyword,
            PhpTokenType.instanceofKeyword,
            PhpTokenType.namespaceKeyword,
            PhpTokenType.newKeyword,
            PhpTokenType.old_functionKeyword,
            PhpTokenType.orKeyword,
            PhpTokenType.privateKeyword,
            PhpTokenType.protectedKeyword,
            PhpTokenType.publicKeyword,
            PhpTokenType.staticKeyword,
            PhpTokenType.switchKeyword,
            PhpTokenType.throwKeyword,
            PhpTokenType.tryKeyword,
            PhpTokenType.useKeyword,
            PhpTokenType.varKeyword,
            PhpTokenType.whileKeyword,
            PhpTokenType.xorKeyword
        };

        private static IEnumerable<PhpTokenType> GetNonKeywordTokenTypes()
        {
            return Enum.GetValues(typeof(PhpTokenType)).Cast<PhpTokenType>()
                .Where(tokenType => !KeywordTokenTypes.Contains(tokenType));
        }

        [Test]
        public void Token_ToString()
        {
            var token = new PhpToken(PhpTokenType.OpenTag, "<?", 0);
            Assert.AreEqual("[0] OpenTag: <?", token.ToString());
        }

        [Test]
        public void Token_IsKeyword_Count()
        {
            Assert.AreEqual(49, KeywordTokenTypes.Count); // brittle test but ensures if extra keywords are added a test will fail
        }

        [Test]
        public void Token_Not_IsKeyword_Count()
        {
            Assert.AreEqual(65, GetNonKeywordTokenTypes().Count()); // brittle test but ensures if extra keywords are added a test will fail
        }

        [Test]
        public void Token_IsKeyword()
        {
            foreach (var tokenType in KeywordTokenTypes)
                Assert.True(new PhpToken(tokenType, "lexeme", 0).IsKeyword());
        }

        [Test]
        public void Token_Not_IsKeyword()
        {
            foreach (var tokenType in GetNonKeywordTokenTypes())
                Assert.False(new PhpToken(tokenType, "lexeme", 0).IsKeyword());
        }
    }
}
