using NUnit.Framework;
using Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhpVH.Tests.Integration
{
    [TestFixture]
    public class ArrayFunctionTests
    {
        private ApiOverrideTester _tester = new ApiOverrideTester();

        [Test, Category("ArrayFunction")]
        public void IsArrayTest()
        {
            _tester.RunBooleanOverrideTest("echo is_array({0});");            
        }

        [Test, Category("ArrayFunction")]
        public void ArrayPushPopTest()
        {
            _tester.RunBooleanOverrideTest(@"
                array_push({0}, 'foo');
                echo array_pop({0}) == 'foo';");
        }

        [Test, Category("ArrayFunction")]
        public void SortTest()
        {
            _tester.RunBooleanOverrideTest(@"
                array_push({0}, 'foo');
                array_push({0}, 'bar');
                sort({0});
                echo {0}[0] == 'bar' && {0}[1] == 'foo';");
        }

        [Test, Category("ArrayFunction")]
        public void CountTest()
        {
            _tester.RunBooleanOverrideTest(@"
                array_push({0}, 'foo');
                array_push({0}, 'bar');
                echo count({0}) == 2;");

            _tester.RunBooleanOverrideTest(@"
                array_push({0}, 'foo');
                echo count({0}) == 1;");

            _tester.RunBooleanOverrideTest(@"
                echo count({0}) == 0;");
        }

        [Test, Category("ArrayFunction")]
        public void ShiftTest()
        {
            _tester.RunBooleanOverrideTest(@"
                array_push({0}, 'foo');
                array_push({0}, 'bar');
                echo array_shift({0}) == 'foo' && array_shift({0}) == 'bar';");
        }

        [Test, Category("ArrayFunction")]
        public void ArrayUniqueTest()
        {
            _tester.RunBooleanOverrideTest(@"
                array_push({0}, 'foo');
                array_push({0}, 'foo');
                echo count(array_unique({0})) == 1;");
        }
    }
}
