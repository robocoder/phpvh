using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Aphid.Tests.Integration
{
    [TestFixture(Category = "AphidList")]
    public class ListTests : AphidTests
    {
        [Test]
        public void ListTest()
        {
            AssertFoo("x = [ 9, 'foo' ]; ret x[1];");
        }

        [Test]
        public void ListTest2()
        {
            Assert9("x = [ 9, 'foo' ]; ret x[0];");
        }

        [Test]
        public void ListTest3()
        {
            AssertFoo("#'Std'; a = [ 'f', 'o', 'o' ]; ret a.aggregate(@(x, y)x + y);");
        }

        [Test]
        public void ListTest4()
        {
            AssertFoo("#'Std'; a = 'foo'; ret a.chars().aggregate(@(x, y)x + y);");
        }
    }
}
