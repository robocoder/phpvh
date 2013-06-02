using System;
using System.Linq;
using NUnit.Framework;

namespace PhpVH.Tests.Integration
{
    [TestFixture(Category = "CodeCoverage")]
    public class CodeCoverageTests : PhpTest
    {
        protected override string GetFolder()
        {
            return "Php\\CodeCoverage";
        }

        private void RunCodeCoverageTest(string testName, decimal expectedValue)
        {
            PhpVHTester tester = null;
            try
            {
                using (tester = new PhpVHTester(
                    testName,
                    "X",
                    new[] { GetFolder() + "\\" + testName + "." + GetExtension() }))
                {
                    tester.CodeCoverage = 2;
                    tester.RunPhpVH();
                    var coverage = tester.LoadCoverage();

                    Assert.AreEqual(2, coverage.Count);

                    var falsePositive = coverage.FirstOrDefault(x => x.Key.EndsWith("\\FalsePositiveCheck.php"));

                    Assert.IsNotNull(falsePositive);
                    Assert.AreEqual((decimal)100.0, falsePositive.Value);

                    coverage.Remove(falsePositive.Key);
                    Assert.AreEqual(expectedValue, coverage.First().Value);
                }
            }
            catch
            {
                if (tester != null)
                {
                    tester.DumpOutput();
                }
                throw;
            }
        }

        [Test(Description = "Code coverage test 1")]
        public void TestBlockCoverage()
        {
            RunCodeCoverageTest("CodeCoverage", 80M);
        }

        [Test(Description = "Code coverage test 2")]
        public void TestBlockCoverage2()
        {
            RunCodeCoverageTest("CodeCoverage2", 100M);
        }

        [Test(Description = "Code coverage test 3")]
        public void TestBlockCoverage3()
        {
            RunCodeCoverageTest("CodeCoverage3", 50M);
        }

        [Test(Description = "Code coverage test 4")]
        public void TestBlockCoverage4()
        {
            RunCodeCoverageTest("CodeCoverage4", 100M);
        }
    }
}
