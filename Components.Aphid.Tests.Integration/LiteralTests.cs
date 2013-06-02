using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Aphid.Tests.Integration
{
    [TestFixture(Category = "AphidLiteral")]
    public class LiteralTests : AphidTests
    {
        [Test]
        public void NumberTest()
        {
            Assert9("ret 9;");
        }

        [Test]
        public void HexNumberTest()
        {
            Assert9("ret 0x9;");
        }

        [Test]
        public void HexNumberTest2()
        {
            Assert9("ret 0xA - 0x1;");
        }

        [Test]
        public void HexNumberTest3()
        {
            Assert9("ret 0xf - 0x6;");
        }

        [Test]
        public void HexNumberTest4()
        {
            AssertEquals(0, "ret 0x0;");
        }

        [Test]
        public void StringTest()
        {
            AssertFoo("ret 'foo';");
        }
    }
}
