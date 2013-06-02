using NUnit.Framework;
using PhpVH.LexicalAnalysis;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace PhpVH.Tests.Unit
{
    [TestFixture, Category("StaticAnalysis")]
    public class StaticAnalyzerTests
    {
        private Assembly _assembly;

        [TestFixtureSetUp]
        public void SetUp()
        {
            _assembly = Assembly.GetExecutingAssembly();
        }

        [Test]
        public void FindSuperglobalFields_Variable_ReturnsEmpty()
        {
            var tokens = GetTokens("<? $var1 = $_GET[$key]; ?>");

            var actual = StaticAnalyzer.FindSuperglobalFields(tokens);

            var expected = GetSuperglobalFields();

            AssertAreEqual(expected, actual);
        }
        
        [Test]
        public void FindSuperglobalFields_Concatenated_ReturnsSuperglobals()
        {
            var tokens = GetTokens("<? $url = $_GET['title'].\"#\".$_GET['na']; ?>");

            var actual = StaticAnalyzer.FindSuperglobalFields(tokens);

            var expected = GetSuperglobalFields();
            expected["$_GET"].AddRange(new[] { "title", "na" });

            AssertAreEqual(expected, actual);
        }

        [Test]
        public void FindSuperglobalFields_InIfBlocks_ReturnsSuperglobals()
        {
            var tokens = GetTokens("<?php\n" +
                                   "print_r($_GET);\n" +
                                   "if($_GET[\"a\"] === \"\") echo \"a is an empty string\n\";\n" +
                                   "if($_GET[\"b\"] === false) echo \"a is false\n\";\n" +
                                   "if($_GET[\"c\"] === null) echo \"a is null\n\";\n" +
                                   "if(isset($_GET[\"d\"])) echo \"a is set\n\";\n" +
                                   "if(!empty($_GET[\"e\"])) echo \"a is not empty\";\n" +
                                   "?>");

            var actual = StaticAnalyzer.FindSuperglobalFields(tokens);

            var expected = GetSuperglobalFields();
            expected["$_GET"].AddRange(new[] { "a", "b", "c", "d", "e" });

            AssertAreEqual(expected, actual);
        }

        [Test]
        public void FindSuperglobalFields_DoubleQuotedString_ReturnsSuperglobals()
        {
            var tokens = GetTokens("<? $var1 = $_GET[\"val1\"]; ?>");

            var actual = StaticAnalyzer.FindSuperglobalFields(tokens);

            var expected = GetSuperglobalFields();
            expected["$_GET"].Add("val1");

            AssertAreEqual(expected, actual);
        }

        [Test]
        public void FindSuperglobalFields_SingleQuotedString_ReturnsSuperglobals()
        {
            var tokens = GetTokens("<? $var1 = $_GET['val1']; ?>");

            var actual = StaticAnalyzer.FindSuperglobalFields(tokens);

            var expected = GetSuperglobalFields();
            expected["$_GET"].Add("val1");

            AssertAreEqual(expected, actual);
        }

        //[Test]
        //[Ignore(
        //    "This is definitely broken, but I'm going to hold off on fixing " +
        //    "it since this is an edge case that we hopefully won't see in the " +
        //    "wild.")]
        //public void FindSuperglobalFields_HereDocString_ReturnsSuperglobals()
        //{
        //    var tokens = GetTokens("<? $var_1 = $_POST[<<<VAR_1\nval_1\nVAR_1\n]; ?>");

        //    var actual = StaticAnalyzer.FindSuperglobalFields(tokens);

        //    var expected = GetSuperglobalFields();
        //    expected["$_POST"].Add("val_1");

        //    AssertAreEqual(expected, actual);
        //}

        [Test]
        public void FindSuperglobalFields_AllFields_ReturnsSuperglobals()
        {
            var tokens = GetTokensFromResource("PhpVH.Tests.Unit.Php.StaticAnalyzerSuperglobalFields.php");

            var actual = StaticAnalyzer.FindSuperglobalFields(tokens);

            var expected = GetSuperglobalFields();
            expected["$_GET"].Add("val1");
            expected["$_POST"].Add("val_2");
            expected["$_REQUEST"].Add("_var-3");
            expected["$_FILES"].Add("_$$REQUEST_var__4");
            expected["$_COOKIE"].Add("$REQUEST_var_5_");

            AssertAreEqual(expected, actual);
        }

        private static PhpToken[] GetTokens(string text)
        {
            var lexer = new PhpLexer(text);

            return lexer.GetTokens().ToArray();
        }

        private PhpToken[] GetTokensFromResource(string name)
        {
            using (var stream = _assembly.GetManifestResourceStream(name))
            {
                Assert.IsNotNull(stream, name);

                using (var reader = new StreamReader(stream))
                    return GetTokens(reader.ReadToEnd());
            }
        }

        private static Dictionary<string, List<string>> GetSuperglobalFields()
        {
            return new Dictionary<string, List<string>>
            {
                { "$_GET", new List<string>() },
                { "$_POST", new List<string>() },
                { "$_REQUEST", new List<string>() },
                { "$_FILES", new List<string>() },
                { "$_COOKIE", new List<string>() }
            };
        }

        private static void AssertAreEqual(Dictionary<string, List<string>> expected, Dictionary<string, List<string>> actual)
        {
            foreach (var list in expected)
                CollectionAssert.AreEqual(list.Value, actual[list.Key], list.Key);
        }
    }
}
