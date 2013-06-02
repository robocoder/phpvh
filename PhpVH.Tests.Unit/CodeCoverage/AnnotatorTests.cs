using System;
using NUnit.Framework;
using PhpVH.CodeCoverage;
using PhpVH.LexicalAnalysis;

namespace PhpVH.Tests.Unit.CodeCoverage
{
    [TestFixture, Category("CodeCoverage")]
    public class AnnotatorTests
    {
        private Annotator _annotator;

        private static readonly Func<string, PhpToken[]> GetTokens = php => new PhpLexer(php).GetTokens().ToArray();

        private const string IfStatementFile = "if";

        private const string IfStatementPhp = "<?if($login=='admin'){$admin=1;}?>";

        private static readonly PhpToken[] IfStatementTokens = GetTokens(IfStatementPhp);

        private const string SwitchStatementFile = "switch";

        private const string SwitchStatementPhp = "<?switch($login[0]){case 'a':if($login=='admin'){$admin=1;}}?>";

        private static readonly PhpToken[] SwitchStatementTokens = GetTokens(SwitchStatementPhp);

        private const string TryCatchStatementFile = "try_catch";

        private const string TryCatchStatementPhp = "<?try{if($login=='admin'){$admin=1;}}catch(Exception $e){}?>";

        private static readonly PhpToken[] TryCatchStatementTokens = GetTokens(TryCatchStatementPhp);

        private const string NoPhpCodeFile = "no_code";

        private const string NoPhpCodePhp = "<html></html>";

        private static readonly PhpToken[] NoPhpCodeTokens = GetTokens(NoPhpCodePhp);

        private const string ClassKeywordFile = "class.php";

        private const string ClassKeywordPhp = "<? class foo {} ?>";

        private static readonly PhpToken[] ClassKeywordTokens = GetTokens(ClassKeywordPhp);

        private const string VariableVariableFile = "variable_variable.php";

        private const string VariableVariablePhp = "<?php\n${'x'} = 'Hello world';\necho $x;\n?>";

        private static readonly PhpToken[] VariableVariableTokens = GetTokens(VariableVariablePhp);

        [SetUp]
        public void Setup()
        {
            _annotator = new Annotator
            {
                Config = ScanConfig.Create(new[] { "c:\\tools\\xampp\\htdocs", "test" })
            };
        }

        [Test]
        public void AnnotateCode_IfStatement_ReturnsCode()
        {
            string annotatedCode = _annotator.AnnotateCode(IfStatementFile, IfStatementPhp, IfStatementTokens);

            Assert.AreEqual("<?\r\nAnnotation(\"if_1\");\r\nif($login=='admin'){\r\nAnnotation(\"if_0\");\r\n$admin=1;}?>", annotatedCode);
        }

        [Test]
        public void AnnotateCode_IfStatement_IncreasesAnnotationIndexes()
        {
            _annotator.AnnotateCode(IfStatementFile, IfStatementPhp, IfStatementTokens);

            Assert.AreEqual(2, _annotator.AnnotationIndexes[IfStatementFile]);
        }

        [Test]
        public void AnnotateCode_IfStatement_AddsNewAnnotationsToAnnotationTable()
        {
            _annotator.AnnotateCode(IfStatementFile, IfStatementPhp, IfStatementTokens);

            var expected = new AnnotationList(IfStatementFile);
            expected.Add(new Annotation(0, 22, 0));
            expected.Add(new Annotation(1, 2, 0));

            Assert.AreEqual(expected, _annotator.AnnotationTable[IfStatementFile]);
        }

        [Test]
        public void AnnotateCode_SwitchStatement_ReturnsCode()
        {
            string annotatedCode = _annotator.AnnotateCode(SwitchStatementFile, SwitchStatementPhp, SwitchStatementTokens);

            Assert.AreEqual("<?\r\nAnnotation(\"switch_1\");\r\nswitch($login[0]){case 'a':if($login=='admin'){\r\nAnnotation(\"switch_0\");\r\n$admin=1;}}?>", annotatedCode);
        }

        [Test]
        public void AnnotateCode_SwitchStatement_IncreasesAnnotationIndexes()
        {
            _annotator.AnnotateCode(SwitchStatementFile, SwitchStatementPhp, SwitchStatementTokens);

            Assert.AreEqual(2, _annotator.AnnotationIndexes[SwitchStatementFile]);
        }

        [Test]
        public void AnnotateCode_SwitchStatement_AddsNewAnnotationsToAnnotationTable()
        {
            _annotator.AnnotateCode(SwitchStatementFile, SwitchStatementPhp, SwitchStatementTokens);

            var expected = new AnnotationList(SwitchStatementFile);
            expected.Add(new Annotation(0, 49, 0));
            expected.Add(new Annotation(1, 2, 0));

            Assert.AreEqual(expected, _annotator.AnnotationTable[SwitchStatementFile]);
        }

        [Test]
        public void AnnotateCode_TryCatchStatement_ReturnsCode()
        {
            string annotatedCode = _annotator.AnnotateCode(TryCatchStatementFile, TryCatchStatementPhp, TryCatchStatementTokens);

            Assert.AreEqual("<?\r\nAnnotation(\"try_catch_3\");\r\ntry{\r\nAnnotation(\"try_catch_2\");\r\nif($login=='admin'){\r\nAnnotation(\"try_catch_1\");\r\n$admin=1;}}catch(Exception $e){\r\nAnnotation(\"try_catch_0\");\r\n}?>", annotatedCode);
        }

        [Test]
        public void AnnotateCode_TryCatchStatement_IncreasesAnnotationIndexes()
        {
            _annotator.AnnotateCode(TryCatchStatementFile, TryCatchStatementPhp, TryCatchStatementTokens);

            Assert.AreEqual(4, _annotator.AnnotationIndexes[TryCatchStatementFile]);
        }

        [Test]
        public void AnnotateCode_TryCatchStatement_AddsNewAnnotationsToAnnotationTable()
        {
            _annotator.AnnotateCode(TryCatchStatementFile, TryCatchStatementPhp, TryCatchStatementTokens);

            var expected = new AnnotationList(TryCatchStatementFile);
            expected.Add(new Annotation(0, 57, 0));
            expected.Add(new Annotation(1, 26, 0));
            expected.Add(new Annotation(2, 6, 0));
            expected.Add(new Annotation(3, 2, 0));

            Assert.AreEqual(expected, _annotator.AnnotationTable[TryCatchStatementFile]);
        }

        [Test]
        public void AnnotateCode_NoPhpCode_ReturnsCode()
        {
            string annotatedCode = _annotator.AnnotateCode(NoPhpCodeFile, NoPhpCodePhp, NoPhpCodeTokens);

            Assert.AreEqual("<?php \r\nAnnotation(\"no_code_0\");\r\n ?><html></html>", annotatedCode);
        }

        [Test]
        public void AnnotateCode_NoPhpCode_IncreasesAnnotationIndexes()
        {
            _annotator.AnnotateCode(NoPhpCodeFile, NoPhpCodePhp, NoPhpCodeTokens);

            Assert.AreEqual(1, _annotator.AnnotationIndexes[NoPhpCodeFile]);
        }

        [Test]
        public void AnnotateCode_NoPhpCode_AddsNewAnnotationToAnnotationTable()
        {
            _annotator.AnnotateCode(NoPhpCodeFile, NoPhpCodePhp, NoPhpCodeTokens);

            var expected = new AnnotationList(NoPhpCodeFile);
            expected.Add(new Annotation(0, 6, 0));

            Assert.AreEqual(expected, _annotator.AnnotationTable[NoPhpCodeFile]);
        }

        [Test]
        public void AnnotateCode_ClassKeyword_ReturnsCode()
        {
            string annotatedCode = _annotator.AnnotateCode(ClassKeywordFile, ClassKeywordPhp, ClassKeywordTokens);

            Assert.AreEqual("<?\r\nAnnotation(\"class.php_0\");\r\n class foo {} ?>", annotatedCode);
        }

        [Test]
        public void AnnotateCode_ClassKeyword_IncreasesAnnotationIndexes()
        {
            _annotator.AnnotateCode(ClassKeywordFile, ClassKeywordPhp, ClassKeywordTokens);

            Assert.AreEqual(1, _annotator.AnnotationIndexes[ClassKeywordFile]);
        }

        [Test]
        public void AnnotateCode_ClassKeyword_AddsNewAnnotationToAnnotationTable()
        {
            _annotator.AnnotateCode(ClassKeywordFile, ClassKeywordPhp, ClassKeywordTokens);

            var expected = new AnnotationList(ClassKeywordFile);
            expected.Add(new Annotation(0, 2, 0));

            Assert.AreEqual(expected, _annotator.AnnotationTable[ClassKeywordFile]);
        }

        [Test]
        public void AnnotateCode_VariableVariable_ReturnsCode()
        {
            string annotatedCode = _annotator.AnnotateCode(VariableVariableFile, VariableVariablePhp, VariableVariableTokens);

            Assert.AreEqual("<?php\r\nAnnotation(\"variable_variable.php_0\");\r\n\n${'x'} = 'Hello world';\necho $x;\n?>", annotatedCode);
        }

        [Test]
        public void AnnotateCode_VariableVariable_IncreasesAnnotationIndexes()
        {
            _annotator.AnnotateCode(VariableVariableFile, VariableVariablePhp, VariableVariableTokens);

            Assert.AreEqual(1, _annotator.AnnotationIndexes[VariableVariableFile]);
        }

        [Test]
        public void AnnotateCode_VariableVariable_AddsNewAnnotationToAnnotationTable()
        {
            _annotator.AnnotateCode(VariableVariableFile, VariableVariablePhp, VariableVariableTokens);

            var expected = new AnnotationList(VariableVariableFile);
            expected.Add(new Annotation(0, 5, 0));

            Assert.AreEqual(expected, _annotator.AnnotationTable[VariableVariableFile]);
        }
    }
}
