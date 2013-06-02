using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Aphid.Tests.Integration
{
    [TestFixture(Category = "AphidOperator")]
    public class OperatorTests : AphidTests
    {
        [Test]
        public void AssignmentTest()
        {
            AssertFoo("x='foo'; ret x;");
        }

        [Test]
        public void AssignmentTest2()
        {
            AssertFoo("x='bar'; x='foo'; ret x;");
        }

        [Test]
        public void StringConcatTest()
        {
            AssertFoo("ret 'fo'+'o';");
        }

        [Test]
        public void AdditionTest()
        {
            Assert9("ret 2+7;");
        }

        [Test]
        public void SubtractionTest()
        {
            Assert9("ret 11-2;");
        }

        [Test]
        public void MultiplicationTest()
        {
            AssertEquals(20m, "ret 10*2;");
        }        
    }
}
