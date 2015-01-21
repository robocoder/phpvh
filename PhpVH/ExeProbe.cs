using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;


namespace PhpVH
{
    public static class ExeProbe
    {
        private const string _probeName = "PHPVHProbe.exe";

        public static void Copy()
        {
            var source = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                _probeName);


            var probes = new[] 
            { 
                Environment.SpecialFolder.System, 
                Environment.SpecialFolder.SystemX86 
            }
                .Select(Environment.GetFolderPath)
                .Where(Directory.Exists)
                .Select(x => Path.Combine(x, _probeName))
                .Where(x => !File.Exists(x));

            foreach (var p in probes)
            {
                Trace.WriteLine("Copying probe");
                File.Copy(source, p);
            }
        }
    }
}
