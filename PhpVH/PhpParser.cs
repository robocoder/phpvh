using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using PhpVH.LexicalAnalysis;

namespace PhpVH
{
    public class PhpParser
	{
        public static PhpToken[] StripWhitespaceAndComments(PhpToken[] tokens)
        {
            return tokens
                .Where(x =>
                    x.TokenType != PhpTokenType.WhiteSpace &&
                    x.TokenType != PhpTokenType.Comment)
                .ToArray();
        }

        public static List<FunctionCall> GetGlobalFunctionCalls(PhpToken[] tokens)
        {
            int state = 0;

            var calls = new List<FunctionCall>();

            FunctionCall call = new FunctionCall();
            List<PhpToken> paramTokens = null;

            for (int i = 0; i < tokens.Length; i++)
            {
                if (state == 0 &&
                    (tokens[i].TokenType == PhpTokenType.ObjectOperator ||
                    tokens[i].TokenType == PhpTokenType.ScopeResolutionOperator))
                    state = -1;
                else if ((state == 0 || state == 1) &&
                    (tokens[i].TokenType == PhpTokenType.Identifier || tokens[i].TokenType == PhpTokenType.Variable))
                {
                    state = 1;

                    call = new FunctionCall();
                    call.Id = tokens[i];
                    paramTokens = new List<PhpToken>();
                }
                else if (state == 1 && tokens[i].TokenType == PhpTokenType.LeftParenthesis)
                {
                    paramTokens.Add(tokens[i]);
                    state = 2;
                }
                else if (state > 1)
                {
                    paramTokens.Add(tokens[i]);

                    if (tokens[i].TokenType == PhpTokenType.LeftParenthesis)
                        state++;
                    else if (tokens[i].TokenType == PhpTokenType.RightParenthesis)
                        state--;

                    if (state <= 1)
                    {
                        call.ParamTokens = paramTokens.ToArray();
                        calls.Add(call);
                        state = 0;
                    }

                }
                else if (tokens[i].TokenType != PhpTokenType.WhiteSpace &&
                    tokens[i].TokenType != PhpTokenType.Comment)
                    state = 0;
            }

            return calls;
        }

        public static PhpToken[] GetGlobalFunctionCallIds(PhpToken[] tokens)
        {
            var functionCalls = new List<PhpToken>();

            int state = 0;

            for (int i = 0; i < tokens.Length; i++)
            {
                if (state == 0 &&
                    (tokens[i].TokenType == PhpTokenType.ObjectOperator ||
                    tokens[i].TokenType == PhpTokenType.ScopeResolutionOperator))
                    state = -1;
                else if ((state == 0 || state == 1) && tokens[i].TokenType == PhpTokenType.Identifier)
                    state = 1;
                else if (state == 1 && tokens[i].TokenType == PhpTokenType.LeftParenthesis)
                {
                    functionCalls.Add(tokens[i - 1]);

                    state = 0;
                }
                else if (tokens[i].TokenType != PhpTokenType.WhiteSpace &&
                    tokens[i].TokenType != PhpTokenType.Comment)
                    state = 0;
            }

            return functionCalls.ToArray();
        }

        public static PhpToken[] GetArrayAccesses(PhpToken[] tokens)
        {
            var functionCalls = new List<PhpToken>();

            int state = 0;

            for (int i = 0; i < tokens.Length; i++)
            {
                if ((state == 0 || state == 1) && tokens[i].TokenType == PhpTokenType.Variable)
                    state = 1;
                else if (state == 1 && tokens[i].TokenType == PhpTokenType.LeftBracket)
                {
                    functionCalls.Add(tokens[i - 1]);

                    state = 0;
                }
                else if (tokens[i].TokenType != PhpTokenType.WhiteSpace &&
                    tokens[i].TokenType != PhpTokenType.Comment)
                    state = 0;
            }

            return functionCalls.ToArray();
        }

        public static PhpToken[] GetBlock(PhpToken[] tokens, int index)
        {
            var start = index;

            if (tokens[index].TokenType != PhpTokenType.LeftBrace)
            {
                throw new InvalidOperationException("Block must begin with a left brace.");
            }

            int depth = 1;

            while (depth != 0)
            {
                var t = tokens[++index];

                if (t.TokenType == PhpTokenType.LeftBrace)
                {
                    depth++;
                }
                else if (t.TokenType == PhpTokenType.RightBrace)
                {
                    depth--;
                }
            }

            var tokenCount = index - start + 1;
            var blockTokens = new PhpToken[tokenCount];
            Array.Copy(tokens, start, blockTokens, 0, tokenCount);

            return blockTokens;
        }

        public static string[] GetIncludedFiles(PhpToken[] tokens)
        {
            var includedFiles = new List<string>();

            for (int i = 0; i < tokens.Length; i++)
                if (Php.IncludeFunctions.Contains(tokens[i].Lexeme))
                {
                    int state = 0;
                    string filename = null;

                    for (int j = i + 1; j < tokens.Length; j++)
                    {
                        if (state == 0 && tokens[j].TokenType == PhpTokenType.LeftParenthesis)
                        {
                            state = 1;
                        }
                        else if (state == 0 && tokens[j].TokenType == PhpTokenType.String)
                        {
                            state = 4;
                            filename = tokens[j].Lexeme;
                        }
                        else if (state == 1 && tokens[j].TokenType == PhpTokenType.String)
                        {
                            state = 2;
                            filename = tokens[j].Lexeme;
                        }
                        else if (state == 2 && tokens[j].TokenType == PhpTokenType.RightParenthesis)
                        {
                            state = 3;
                            break;
                        }
                        else if (state == 4 && tokens[j].TokenType == PhpTokenType.EndOfStatement)
                        {
                            state = 3;
                            break;
                        }
                        else
                            break;
                    }

                    if (state == 3)
                    {
                        includedFiles.Add(filename.Substring(1, filename.Length - 2));
                    }
                }

            return includedFiles.ToArray();
        }

        public static Dictionary<string, List<string>> GetSuperglobalFields(PhpToken[] tokens)
        {
            var fieldDictionary = Php.Superglobals
                .ToDictionary(x => x, x => new List<string>());

            for (int i = 0; i < tokens.Length; i++)
                if (Php.Superglobals.Contains(tokens[i].Lexeme))
                {
                    int state = 0;
                    string fieldName = null;

                    for (int j = i + 1; j < tokens.Length; j++)
                    {
                        if (state == 0 && tokens[j].TokenType == PhpTokenType.LeftBracket)
                        {
                            state = 1;
                        }
                        else if (state == 1 && tokens[j].TokenType == PhpTokenType.String)
                        {
                            state = 2;
                            fieldName = tokens[j].Lexeme;
                        }
                        else if (state == 2 && tokens[j].TokenType == PhpTokenType.RightBracket)
                        {
                            state = 3;
                            break;
                        }
                        else
                            break;
                    }

                    if (state == 3)
                    {
                        var name = fieldName.Substring(1, fieldName.Length - 2);

                        if (!fieldDictionary[tokens[i].Lexeme].Contains(name))
                            fieldDictionary[tokens[i].Lexeme].Add(name);
                    }
                }

            return fieldDictionary;
        }        
	}
}
