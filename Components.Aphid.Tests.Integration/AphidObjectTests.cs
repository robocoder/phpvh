using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Aphid.Tests.Integration
{
    [TestFixture(Category = "AphidObject")]
    public class AphidObjectTests : AphidTests
    {
        [Test]
        public void MemberShortHandTest()
        {
            AssertFoo("y = 'foo'; x = { y }; ret x.y;");
        }

        [Test]
        public void DynamicMemberTest()
        {
            AssertFoo("x = { y: 'foo' }; ret x.{'y'};");
        }

        [Test]
        public void DynamicMemberTest2()
        {
            AssertFoo("x = { y: { z: 'foo' } }; ret x.{'y'}.z;");
        }

        [Test]
        public void DynamicMemberTest3()
        {
            AssertFoo("x = { y: { z: 'foo' } }; ret x.{'y'}.{'z'};");
        }
    }
}
