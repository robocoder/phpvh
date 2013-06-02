using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Components;
using System.Diagnostics;
using NUnit.Framework;
using System.Text.RegularExpressions;

namespace PhpVH.Tests.Integration
{
    public class PhpTester
    {
        private string[] GetInterpreters()
        {
            var dir = ConfigurationManager.AppSettings["interpreterPath"];

            Assert.True(Directory.Exists(dir), "{0} not found", dir);

            var interpeters = Directory.GetFiles(dir, "php.exe", SearchOption.AllDirectories);

            CollectionAssert.IsNotEmpty(
                interpeters,
                "No PHP interpreters found in {0}.",
                dir);

            return interpeters;
        }

        public void RunInterpreters(string code, Action<string> outputCallback)
        {
            GetInterpreters().Iter(x =>
            {
                var codeFile = Path.GetTempFileName();
                File.WriteAllText(codeFile, code);
                var p = Process.Start(new ProcessStartInfo(x, string.Format("-f {0}", codeFile))
                {
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                });

                var output = p.StandardOutput.ReadToEnd() + p.StandardError.ReadToEnd();
                p.WaitForExit();
                File.Delete(codeFile);

                outputCallback(output);
            });
        }

        public void RunBooleanTest(string code)
        {
            RunInterpreters(code, x => 
            {
                var msg = string.Format("Output:\r\n{0}\r\n\r\nCode:\r\n{1}", x, code);

                if (x != "1")
                {
                    bool b;
                    Assert.IsTrue(bool.TryParse(x, out b), msg);
                    Assert.IsTrue(b, msg);
                }
            });
        }
    }
}
