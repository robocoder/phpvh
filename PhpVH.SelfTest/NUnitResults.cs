using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PhpVH.SelfTest
{
    public class NUnitResults
    {
        private XElement _xElement;

        private NUnitResults()
        {
        }

        public static NUnitResults Load(string uri)
        {
            return new NUnitResults()
            {
                _xElement = XElement.Load(uri),
            };
        }

        public IEnumerable<XElement> GetTestCases()
        {
            return _xElement.Descendants("test-case");
        }
    }
}
