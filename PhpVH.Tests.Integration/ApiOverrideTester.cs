using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Components;

namespace PhpVH.Tests.Integration
{
    public class ApiOverrideTester
    {
        private string PatchCode(string code)
        {
            code = PreloadHelper.PatchArrayFunctions("<?php  \r\n$a=array();" + code + "?>");
            var include =
                File.ReadAllText("PhpResources\\SuperGlobalOverride.php") +
                File.ReadAllText("PhpResources\\PhpApiOverrides.php");
            code = code.Insert(6, include);
            return code;
        }

        public void RunBooleanOverrideTest(string template)
        {
            var tester = new PhpTester();
            GetArrays().Iter(x => tester.RunBooleanTest(PatchCode(string.Format(template, x))));
        }

        public IEnumerable<string> GetArrays()
        {
            return Php.Superglobals.Concat(new[] { "$a" });
        }
    }
}
