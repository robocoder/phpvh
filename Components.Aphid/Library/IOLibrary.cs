using Components.Aphid.Interpreter;
using System;
using System.IO;

namespace Components.Aphid.Library
{
    [AphidLibraryAttribute("io")]
    public class IOLibrary
    {
        [AphidInteropFunction("io.read")]
        private static string Read(string filename)
        {
            return File.ReadAllText(filename);
        }

        [AphidInteropFunction("io.write")]
        private static void Write(string filename, string text)
        {
            File.WriteAllText(filename, text);
        }
    }
}

