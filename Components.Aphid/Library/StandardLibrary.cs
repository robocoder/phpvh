using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Components.Aphid.Interpreter;
using System.Text.RegularExpressions;

namespace Components.Aphid.Library
{
    [AphidLibraryAttribute("standard")]
    public class StandardLibrary
    {
        [AphidInteropFunction("eval", PassInterpreter = true)]
        private static object Eval(AphidInterpreter interpreter, string code)
        {
            interpreter.EnterChildScope();
            interpreter.Interpret(code);
            var retVal = interpreter.GetReturnValue();
            interpreter.LeaveChildScope();
            return retVal;
        }

        [AphidInteropFunction("print")]
        private static void Print(object message)
        {
            Console.WriteLine(message != null ? message.ToString() : null);
        }

        [AphidInteropFunction("sprintf")]
        private static string SPrintF(string format, params object[] args)
        {
            return string.Format(format, args);
        }

        [AphidInteropFunction("input")]
        private static string ReadLine()
        {
            return Console.ReadLine();
        }

        [AphidInteropFunction("str")]
        private static string ConvertToString(object obj)
        {
            return Convert.ToString(obj);
        }

        [AphidInteropFunction("asc")]
        private static decimal ConvertToCharCode(object obj)
        {
            if (obj is string)
            {
                var str = (obj as string);

                if (str.Length != 0)
                {
                    throw new InvalidOperationException();
                }

                return (decimal)str[0];
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        [AphidInteropFunction("chr")]
        private static string ConvertToCharCode(decimal obj)
        {
            return ((char)obj).ToString();
        }

        [AphidInteropFunction("hexb")]
        private static string ConvertToHexByteString(object value)
        {
            if (value is decimal)
            {
                return ConvertToHexByteString((decimal)value);
            }
            else if (value is string)
            {
                var s = value as string;

                if (s.Length != 1)
                {
                    throw new InvalidOperationException();
                }

                return Convert.ToString(s[0], 16).PadLeft(2, '0');
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        private static string ConvertToHexByteString(decimal value)
        {
            return Convert.ToString((byte)value, 16).PadLeft(2, '0');
        }

        private static Random _random = new Random();

        [AphidInteropFunction("randInt")]
        public static decimal RandomInt(decimal minValue, decimal maxValue)
        {
            lock (_random)
            {
                return (decimal)_random.Next((int)minValue, (int)maxValue);
            }
        }

        [AphidInteropFunction("range")]
        private static List<AphidObject> Range(decimal start, decimal count)
        {
            return Enumerable
                .Range((int)start, (int)count)
                .Select(x => new AphidObject((decimal)x))
                .ToList();
        }

        [AphidInteropFunction("__list.add")]
        private static void ListAdd(List<AphidObject> list, object value)
        {
            list.Add (new AphidObject(value));
        }

        [AphidInteropFunction("__list.contains")]
        private static bool ListContains(List<AphidObject> list, object value)
        {
            var s = list.Any (x => x.Value.Equals(value));
            return  s;
        }

        [AphidInteropFunction("__list.insert")]
        private static void ListAdd(List<AphidObject> list, decimal index, object value)
        {
            list.Insert((int)index, new AphidObject(value));
        }

        [AphidInteropFunction("__list.count")]
        private static decimal ListAdd(List<AphidObject> list)
        {
            return (decimal)list.Count;
        }

        [AphidInteropFunction("__string.length")]
        private static decimal StringLength(string str)
        {
            return (decimal)str.Length;
        }

        [AphidInteropFunction("__string.getChars")]
        private static List<AphidObject> StringGetChars(string str)
        {
            return str.Select(x => new AphidObject(x.ToString())).ToList();
        }

        [AphidInteropFunction("__string.remove")]
        private static string StringGetChars(string str, decimal index)
        {
            return str.Remove((int)index);
        }

        [AphidInteropFunction("__string.substring", UnwrapParameters = false)]
        private static string StringSubstring(AphidObject str, AphidObject index, AphidObject length)
        {
            var str2 = (string)str.Value;
            return length == null ? 
                str2.Substring((int)(decimal)index.Value) :
                str2.Substring((int)(decimal)index.Value, (int)(decimal)length.Value);
        }

        [AphidInteropFunction("__string.startsWith")]
        private static bool StringStartsWith(string str, string value)
        {
            return str.StartsWith(value);
        }

        [AphidInteropFunction("__string.endsWith")]
        private static bool StringEndsWith(string str, string value)
        {
            return str.EndsWith(value);
        }

        [AphidInteropFunction("__string.trim")]
        private static string StringTrim(string str)
        {
            return str.Trim();
        }        

        [AphidInteropFunction("__string.split")]
        private static List<AphidObject> StringSplit(string str, List<AphidObject> separator)
        {
            var s = separator.Select(x => x.GetString()).ToArray();
            return str
                .Split(s, StringSplitOptions.None)
                .Select(x => new AphidObject(x))
                .ToList();
        }

        [AphidInteropFunction("__string.isMatch")]
        private static bool StringIsMatch(string str, string pattern)
        {
            return Regex.IsMatch(str, pattern);
        }
    }
}

