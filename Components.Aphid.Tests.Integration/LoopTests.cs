using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Aphid.Tests.Integration
{
    [TestFixture(Category = "AphidLoop")]
    public class LoopTests : AphidTests
    {
        [Test]
        public void ForTest()
        {
            AssertFoo("c=['f','o','o']; s=''; for(x in c){s = s + x;} ret s;");
        }

        [Test]
        public void ForTest2()
        {
            AssertFoo("c=['f','o','o']; s=''; for(x in c)s = s + x; ret s;");
        }
    }
}
