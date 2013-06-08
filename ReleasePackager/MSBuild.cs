using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReleasePackager
{
    public class MSBuild
    {
        private static string _buildExe = Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings["buildExe"]);

        public static void Build(string csProj)
        {
            var argsFormat = "/t:rebuild /p:Configuration=Debug {0}";
            var args = string.Format(argsFormat, csProj);

            var p = Process.Start(new ProcessStartInfo(_buildExe, args)
            {
                UseShellExecute = false,
            });

            p.WaitForExit();
            
            if (p.ExitCode != 0)
            {
                var msg = string.Format("Process exited with code 0x{0:x8} ({0})", p.ExitCode);
                throw new InvalidOperationException(msg);
            }
        }
    }
}
