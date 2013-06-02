using Components.ConsolePlus;
using Components.Aphid.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhpVH
{
    public static class ScriptLauncher
    {
        private static Dictionary<string, AphidObject> _executeCache = new Dictionary<string, AphidObject>();

        private static Dictionary<AphidObject, string[]> _testCaseCache = new Dictionary<AphidObject, string[]>();

        public static bool IsExecuteCached(string filename)
        {
            return _executeCache.ContainsKey(filename);
        }

        public static AphidObject Execute(string filename)
        {
            AphidObject obj;

            if (_executeCache.TryGetValue(filename, out obj))
            {
                return obj;
            }

            var interpreter = new AphidInterpreter();
            Cli.WriteLine("Executing ~Cyan~{0}~R~", filename);
            interpreter.InterpretFile(filename);            
            obj = interpreter.GetReturnValue();;
            _executeCache.Add(filename, obj);

            return obj;
        }

        public static string[] LoadTestCases(AphidObject obj)
        {
            string[] testCases;

            if (_testCaseCache.TryGetValue(obj, out testCases))
            {
                return testCases;
            }

            testCases = AphidObjectConverter.ToStringArray(obj);
            Cli.WriteLine("~Green~{0}~R~ test cases loaded", testCases.Length);
            return testCases;
        }

        public static string[] LoadTestCases(string filename)
        {
            return LoadTestCases(ScriptLauncher.Execute(filename));
        }
    }
}
