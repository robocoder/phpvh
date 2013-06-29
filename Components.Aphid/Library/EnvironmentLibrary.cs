using Components.Aphid.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components.Aphid.Library
{
    public class EnvironmentLibrary
    {
        [AphidInteropFunction("env.expand")]
        public static string EnvExpand(string str)
        {
            return Environment.ExpandEnvironmentVariables(str);
        }

        private static string[] _envArgs;

        public static void SetEnvArgs(bool skipFirst = false)
        {
            _envArgs = Environment.GetCommandLineArgs();

            if (skipFirst)
            {
                _envArgs = _envArgs.Skip(1).ToArray();
            }
        }

        [AphidInteropFunction("env.args", UnwrapParameters = false)]
        public static List<AphidObject> EnvArgs()
        {
            if (_envArgs == null)
            {
                SetEnvArgs();
            }

            return _envArgs.Select(x => new AphidObject(x)).ToList();
        }
    }
}
