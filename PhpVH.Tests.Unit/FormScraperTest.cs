using NUnit.Framework;

namespace PhpVH.Tests.Unit
{
    [TestFixture, Category("FormScraper")]
    public class FormScraperTest : HtmlTest
    {
        private FormTag[] GetForms(string testFile)
        {
            return FormScraper.GetForms(LoadResource(testFile), "GET");
        }

        [Test]
        public void GetFormsTestMethod()
        {
            var forms = GetForms("Form1");
            Assert.AreEqual(1, forms.Length);
        }

        [Test]
        public void GetInputsTestMethod()
        {
            var inputs = GetForms("Form1")[0].Inputs;
            Assert.AreEqual(2, inputs.Length);
        }

        [Test]
        public void GetAttributesTestMethod()
        {
            var inputs = GetForms("Form1")[0].Inputs;
            Assert.AreEqual("testType", inputs[0].Type);
            Assert.AreEqual("testValue", inputs[0].Value);
            Assert.AreEqual("testType2", inputs[1].Type);
            Assert.AreEqual("testValue2", inputs[1].Value);
        }
    }
}
