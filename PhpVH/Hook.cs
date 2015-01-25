using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Components;
using PhpVH.LexicalAnalysis;

namespace PhpVH
{
    public class Hook
    {
        private static MD5CryptoServiceProvider _md5 = new MD5CryptoServiceProvider();        

        public const string HookFileName = "fern.php";

        public const string TraceFile = "trace.txt";

        public string FunctionName { get; set; }

        public string ReplacementName { get; set; }

        public string ReplacementFunction { get; set; }

        public char StartDelimiter { get; set; }

        public char EndDelimiter { get; set; }

        public Hook(string FunctionName, int Parameters)            
            : this(FunctionName, Parameters, DelimiterOptions.Parentheses)
        {
            
        }

        public Hook(string FunctionName, int Parameters, DelimiterOptions DelimiterOption)
        {
            this.FunctionName = FunctionName;
            ReplacementName = "Hook_" + _md5.ComputeHash(FunctionName) + Parameters;

            var d = new Delimiters(DelimiterOption);

            this.StartDelimiter = d.StartDelimiter;
            this.EndDelimiter = d.EndDelimiter;

            StringBuilder funcParams = new StringBuilder();
            StringBuilder echoes = new StringBuilder();

            string fileOpen = "$f = fopen($_SERVER['DOCUMENT_ROOT'] . '/" +
                TraceFile + "', 'a') or die('error opening trace file');\r\n",
                fileClose = "fclose($f);\r\n";

            string callInfoStart = new string('-', 32) + "Start\\n",
                callInfoEnd = new string('-', 32) + "End\\n";

            string startEcho = "fwrite($f, \"" + callInfoStart + "\");\r\n",
                endEcho = "fwrite($f, \"" + callInfoEnd + "\");\r\n";

            for (int i = 0; i < Parameters; i++)
            {
                string paramName = "$Param" + i.ToString();

                echoes.AppendLine("fwrite($f, '" + paramName + ": '." + paramName + ".\"\\n\");");
                funcParams.Append((funcParams.Length != 0 ? "," : "") + paramName);
            }

            ReplacementFunction =
                DelimiterOption == DelimiterOptions.Parentheses ?
                "function " + ReplacementName + "(" + funcParams + ")\r\n{\r\n" +
                fileOpen +
                startEcho +
                "fwrite($f, \"Function Called: " + FunctionName + "\\n\");\r\n" +
                echoes.ToString() +
                endEcho +
                fileClose +
                "return " + FunctionName + StartDelimiter + funcParams + EndDelimiter + ";\r\n}\r\n" :
                
                "function " + ReplacementName + "(" + funcParams + ")\r\n{\r\n" +
                fileOpen +
                startEcho +
                "fwrite($f, \"Function Called: \\" + FunctionName + "\\n\");\r\n" +
                echoes.ToString() +
                "fwrite($f, \"Value: \");\r\n" +
                //"$e=var_export(" + FunctionName + "[$Param0]);fwrite($f, $e);\r\n" +
                "if (isset(" + FunctionName + "[$Param0])) { fwrite($f, is_array(" + FunctionName + "[$Param0]) ? " +
                    "implode(',', " + FunctionName + "[$Param0]) : " + FunctionName + "[$Param0]); }\r\n" +
                "fwrite($f, \"\\n\");\r\n" +
                endEcho +
                fileClose +
                "return $Param0;\r\n}\r\n";

            if (DelimiterOption == DelimiterOptions.None)
                ReplacementFunction = "";
            
        }

        public void Set(ref string PHP)
        {
            var lexer = new PhpLexer(PHP);
            var tokens = lexer.GetTokens().ToArray();
            var unk = tokens.Where(x => x.TokenType == PhpTokenType.Unknown);

#if DEBUG
            if (unk.Any())
                throw new InvalidOperationException();
#endif

            var funcTokens = StartDelimiter == '(' ?
                PhpParser.GetGlobalFunctionCallIds(tokens) :
                PhpParser.GetArrayAccesses(tokens);            

            for (int i = funcTokens.Length - 1; i >= 0; i--)
            {
                if (funcTokens[i].Lexeme != FunctionName)
                    continue;

                if (StartDelimiter == '(')
                {
                    PHP = PHP.Remove(funcTokens[i].Index, funcTokens[i].Lexeme.Length);
                    PHP = PHP.Insert(funcTokens[i].Index, ReplacementName);                    
                }
                else
                {
                    var tokenIndex = Array.IndexOf(tokens, tokens.Single(x => x.Index == funcTokens[i].Index));
                    PhpToken leftBracket = new PhpToken(), rightBracket = new PhpToken();
                    for (int j = tokenIndex + 1; j < tokens.Length; j++)
                    {
                        if (tokens[j].TokenType == PhpTokenType.LeftBracket)
                            leftBracket = tokens[j];
                        else if (tokens[j].TokenType == PhpTokenType.RightBracket)
                        {
                            rightBracket = tokens[j];
                            break;
                        }
                    }
                    if (leftBracket.TokenType != PhpTokenType.LeftBracket)
                        continue;

                    PHP = PHP.Insert(rightBracket.Index, ")");
                    PHP = PHP.Insert(leftBracket.Index + 1, ReplacementName + "(");                    
                }
            }
        }

        public static Hook[] GetDefaults()
        {
            return new[]
            {
                new Hook(PhpName.Eval, 1),

                new Hook(PhpName.System, 1),
                new Hook(PhpName.System, 2),
                    
                new Hook(PhpName.Exec, 1),
                new Hook(PhpName.Exec, 2),
                new Hook(PhpName.Exec, 3),

                new Hook(PhpName.ShellExec, 1),

                new Hook(PhpName.PassThru, 1),
                new Hook(PhpName.PassThru, 2),
                    
                new Hook(PhpName.MoveUploadedFile, 2),

                new Hook(PhpName.Files, 1, DelimiterOptions.Brackets),
            };
        }

        public static Hook[] GetSuperglobals()
        {
            return new[]
            {
                new Hook(PhpName.Get, 1, DelimiterOptions.Brackets),
                new Hook(PhpName.Post, 1, DelimiterOptions.Brackets),
                new Hook(PhpName.Request, 1, DelimiterOptions.Brackets),
                new Hook(PhpName.Cookie, 1, DelimiterOptions.Brackets),
            };
        }
    }
}
