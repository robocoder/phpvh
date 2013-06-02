using Components;
using Components.Aphid.Interpreter;
using Components.ConsolePlus;
using NUnit.Util;
using PhpVH.Tests.Integration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PhpVH.SelfTest
{
    class Program
    {
        static void WriteHeader(string header)
        {
            var headerStyle = string.Format("~{0}~~|{1}~", ConsoleColor.Blue, ConsoleColor.White);
            Cli.WriteHeader(header, headerStyle);
        }

        static int RunNUnit(SelfTestAssembly asm, SelfTest test)
        {
            return NUnit.ConsoleRunner.Runner.Main(new[] 
            { 
                asm.Assembly, 
                test.Arg,                
                "/nologo",
                "/trace:Off",
                "/domain:none",
                "/out:TestResult.txt",
                "/err:TestErr.txt",
            });
        }

        static TestResult ProcessTestResults(SelfTest test)
        {
            var results = NUnitResults.Load(PathHelper.GetEntryPath("TestResult.xml"));
            var testCases = results
                .GetTestCases()
                .Select(x => new
                {
                    Element = x,
                    Description = x.Attribute("description") != null ? x.Attribute("description").Value : null,
                    Success = bool.Parse(x.Attribute("success").Value),
                    Messages = x.Descendants("message").Select(y => y.Value).ToArray()
                })
                .ToArray();

            var succeeded = testCases.Count(x => x.Success);
            var total = testCases.Count();

            var msg =
                succeeded == 0 ? test.Messages[0] :
                succeeded != total ? test.Messages[1] :
                test.Messages[2];            

            var details = testCases.SelectMany(x => x.Messages).DefaultIfEmpty().Aggregate((x, y) => x + "\r\n" + y);            

            return new TestResult(test.Name, msg, details, testCases.All(x => x.Success), succeeded, total);
        }

        static TestResult RunTest(SelfTestAssembly asm, SelfTest test)
        {
            WriteHeader(string.Format("PHPVH Self Test: {0}", test.Name));
            Cli.WriteLine();
            RunNUnit(asm, test);
            
            return ProcessTestResults(test);
        }

        private static SelfTestAssembly[] LoadAssemblies()
        {
            var interpreter = new AphidInterpreter();
            interpreter.InterpretFile(PathHelper.GetEntryPath("Tests.alx"));
            var retVal = interpreter.GetReturnValue();
            return retVal.ConvertToArray<SelfTestAssembly>();
        }        

        private static List<TestResult> GetTestResults()
        {
            return ServiceLocator.Default.ResolveOrCreate<List<TestResult>>();
        }

        private static void DisplayTestResult(TestResult testCase)
        {
            var successPct = testCase.Passed != 0 ? (100 * ((decimal)testCase.Passed / testCase.Total)) : 0;
            var result = testCase.Succeeded ? "Success" : "Fail";
            var color = testCase.Succeeded ? ConsoleColor.Green : ConsoleColor.Red;
            Cli.WriteLine("~{0}~{1}~R~", color, testCase.Message ?? testCase.Name);

            if (!testCase.Succeeded)
            {
                Cli.WriteLine("~Yellow~{0}~R~", testCase.Details);
            }

            Cli.WriteLine();
        }

        private static void DisplayTestResults()
        {
            WriteHeader("Test Results");
            GetTestResults().Iter(DisplayTestResult);
        }

        [STAThread]
        static void Main(string[] args)
        {
            var results = GetTestResults();            
            
            var pairs = LoadAssemblies().SelectMany(x => x.Tests.Select(y => new { Asm = x, Test = y }));

            foreach (var pair in pairs)
            {
                if (pair.Test.Required != null)
                {
                    var requiredResults = results.Where(x => pair.Test.Required.Contains(x.Name));
                    var succeeded = requiredResults.All(x => x.Succeeded);
                    var failedOrSkipped = requiredResults.Where(x => !x.Succeeded || x.Skipped);

                    if (!succeeded)
                    {
                        var msg = string.Format(
                            "Skipped because the following tests failed or were skipped: {0}",
                            string.Join(", ", failedOrSkipped.Select(x => x.Name)));

                        results.Add(new TestResult() 
                        { 
                            Name = pair.Test.Name, 
                            Message = string.Format("{0} test skipped.", 
                            pair.Test.Name), 
                            Skipped = true, 
                            Details = msg 
                        });

                        Cli.WriteLine("Skipping ~Yellow~{0} Test~R~\r\n", pair.Test.Name);
                        continue;
                    }
                }

                results.Add(RunTest(pair.Asm, pair.Test));
            }            

            DisplayTestResults();

            if (Environment.UserInteractive)
            {
                Cli.Write("Done. Press any key to continue...");
                Console.ReadKey();
            }
        }
    }
}
