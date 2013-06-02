using Components.Aphid;
using Components.Aphid.Interpreter;
using Components.Aphid.Lexer;
using Components.Aphid.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aphid
{
    class Program
    {
        static void DisplayDirections()
        {
            Console.WriteLine("aphid [Script]");
            Environment.Exit(0);
        }

        static string GetCodeExcerpt(string code, AphidToken token)
        {
            var matches = Regex.Matches(code, @"(\r\n)|\r|\n").OfType<Match>().ToArray();
            var firstAfter = matches.FirstOrDefault(x => x.Index > token.Index);

            int line;

            if (firstAfter != null)
            {
                line = Array.IndexOf(matches, firstAfter);
            }
            else
            {
                line = matches.Count();
            }

            var lines = code.Replace("\r\n", "\n").Replace('\r', '\n').Replace("\n", "\r\n").Split(new[] { "\r\n" }, StringSplitOptions.None);
            var loc = lines[line];

            return string.Format("({0}) {1}", line, loc);
        }

        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                DisplayDirections();
            }
            else if (!File.Exists(args[0]))
            {
                Console.WriteLine("Could not find {0}", args[0]);
                Environment.Exit(1);
            }

            var code = File.ReadAllText(args[0]);

            var interpreter = new AphidInterpreter();

            try
            {
                interpreter.Interpret(code);
            }
            catch (AphidParserException exception)
            {
                Console.WriteLine("Unexpected {0}\r\n\r\n{1}\r\n", exception.Token.Lexeme, GetCodeExcerpt(code, exception.Token));
            }
            catch (AphidRuntimeException exception)
            {
                Console.WriteLine("Unexpected runtime exception\r\n\r\n{0}\r\n", exception.Message);
            }
        }
    }
}
