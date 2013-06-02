using System.Text.RegularExpressions;
using NUnit.Framework;

namespace PhpVH.Tests.Unit
{
    [TestFixture, Category("UriScraper")]
    public class UriScraperTest : HtmlTest
    {
        private UriScraper GetScraper()
        {
            return new UriScraper()
            {
                Regex = new Regex(
                    @"[/.]localhost($|/)",
                    RegexOptions.IgnoreCase | RegexOptions.Compiled),
            };
        }

        private string[] ScrapeUris(string testName)
        {
            var html = LoadResource(testName);
            return GetScraper().Parse(html, "http://localhost/");
        }

        private void RunUriTest(string testName, string expectedUri)
        {
            var uris = ScrapeUris(testName);
            Assert.AreEqual(1, uris.Length);
            Assert.AreEqual(expectedUri, uris[0]);
        }

        [Test]
        public void MailToUriTest()
        {
            Assert.AreEqual(0, ScrapeUris("MailTo").Length);
        }

        [Test]
        public void JavaScriptUriTest()
        {
            Assert.AreEqual(0, ScrapeUris("JavaScriptUri").Length);
        }

        [Test]
        public void RelativeUriTest()
        {
            RunUriTest("RelativeUri", "http://localhost/");
        }

        [Test]
        public void RelativeUriTest2()
        {
            RunUriTest("RelativeUri2", "http://localhost/foo.php");
        }

        [Test]
        public void RelativeUriTest3()
        {
            RunUriTest("RelativeUri3", "http://localhost/foo/bar.php");
        }

        [Test]
        public void AbsoluteUriTest()
        {
            RunUriTest("AbsoluteUri", "http://localhost/foo.php");
        }

        [Test]
        public void AbsoluteUriTest2()
        {
            RunUriTest("AbsoluteUri2", "http://localhost/foo.php");
        }

        [Test]
        public void AbsoluteUriTest3()
        {
            RunUriTest("AbsoluteUri3", "http://localhost/foo.php");
        }

        [Test]
        public void UriLikeTest()
        {
            Assert.AreEqual(0, ScrapeUris("UriLike").Length);
        }
    }
}
