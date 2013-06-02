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
            var source = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName + "\\" + _probeName;
            var destination = Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\" + _probeName;

            if (!File.Exists(destination))
            {
                Trace.WriteLine("Copying probe");

                File.Copy(source, destination, true);
            }

        }
    }
}
