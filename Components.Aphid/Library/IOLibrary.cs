using Components.Aphid.Interpreter;
using System;
using System.IO;
using System.Collections.Generic;

namespace Components.Aphid.Library
{
    [AphidLibraryAttribute("io")]
    public class IOLibrary
    {
        [AphidInteropFunction("io.readText")]
        private static string ReadText(string filename)
        {
            return File.ReadAllText(filename);
        }

        [AphidInteropFunction("io.writeText")]
        private static void WriteText(string filename, string text)
        {
            File.WriteAllText(filename, text);
        }

        [AphidInteropFunction("io.readBytes")]
        private static List<AphidObject> ReadBytes(string filename)
        {
            var list = new List<AphidObject>();
            
            foreach (var b in File.ReadAllBytes(filename))
            {
                list.Add(new AphidObject((decimal)b));
            }

            return list;
        }

        [AphidInteropFunction("io.writeBytes")]
        private static void WriteBytes(string filename, List<AphidObject> bytes)
        {
            var buffer = new byte[bytes.Count];

            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = Convert.ToByte(bytes[i].Value);
            }

            File.WriteAllBytes(filename, buffer);
        }
    }
}

