using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Components;

namespace ComponentsUnitTests
{
    [TestClass]
    public class ExtensionUnitTests
    {
        [TestMethod, TestCategory("GeneralExtensions")]
        public void StringToBytesTestMethod()
        {
            var sb = new StringBuilder();
            
            for (var i = 0; i < 256; i++)
            {
                sb.Append((char)i);
            }
            
            var s = sb.ToString();
            var b = s.GetBytes();

            for (var i = 0; i < 256; i++)
            {
                Assert.AreEqual((byte)i, b[i]);
            }
        }

        [TestMethod, TestCategory("GeneralExtensions")]
        public void BytesToStringTestMethod()
        {
            var b = new byte[256];

            for (var i = 0; i < 256; i++)
            {
                b[i] = (byte)i;
            }

            var s = b.GetString();

            for (var i = 0; i < 256; i++)
            {
                Assert.AreEqual((byte)i, (byte)s[i]);
            }
        }
    }
}
