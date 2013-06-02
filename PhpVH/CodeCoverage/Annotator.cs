using System.Collections.Generic;
using System.IO;
using System.Linq;
using PhpVH.LexicalAnalysis;

namespace PhpVH.CodeCoverage
{
    public class Annotator
    {
        public FileInfo AnnotationFile { get; set; }

        public Dictionary<string, int> AnnotationIndexes { get; private set; }

        public AnnotationTable AnnotationTable { get; set; }

        public ScanConfig Config { get; set; }

        public Annotator()
        {
            AnnotationIndexes = new Dictionary<string, int>();
            AnnotationTable = new AnnotationTable();
            Config = Program.Config;
        }

        public string Annotate(string file, string code, int startIndex)
        {
            int x;

            lock (AnnotationIndexes)
            {
                x = AnnotationIndexes[file]++;
                AnnotationTable[file].Add(new Annotation(x, startIndex));
            }

            code = code.Insert(
                startIndex,
                string.Format("\r\nAnnotation(\"{0}_{1}\");\r\n",
                file.Replace("\\", "\\\\"), x));

            return code;
        }

        public string AnnotateBlock(string file, string code, PhpToken[] tokens,
            int tokenIndex, bool functionsOnly)
        {
            if (tokens[tokenIndex - 1].TokenType != PhpTokenType.RightParenthesis &&
                !tokens[tokenIndex - 1].IsKeyword())
            {
                return code;
            }

            for (int i = tokenIndex; i >= 0; i--)
            {
                if (tokens[i].IsKeyword())
                {
                    if (tokens[i].Lexeme == "switch" ||
                        (functionsOnly && tokens[i].Lexeme != "function"))
                        return code;

                    break;
                }
            }

            code = Annotate(file, code, tokens[tokenIndex].Index + 1);

            return code;
        }

        public string AnnotateCode(string file, string php, PhpToken[] tokens)
        {
            lock (AnnotationIndexes)
            {
                AnnotationIndexes.Add(file, 0);
                AnnotationTable.Add(file);
            }

            for (int i = tokens.Length - 1; i >= 0; i--)
            {
                if (tokens[i].TokenType == PhpTokenType.LeftBrace)
                {
                    php = AnnotateBlock(file, php,
                        tokens, i, Config.CodeCoverageReport == 1);
                }
            }

            var open = tokens.FirstOrDefault(x => x.TokenType == PhpTokenType.OpenTag);

            if (open.Lexeme != null)
            {
                php = Annotate(file, php, open.Index + open.Lexeme.Length);
            }
            else
            {
                var tmp = Annotate(file, "<?php  ?>", 6);
                php = tmp + php;
            }

            return php;
        }

        public void Reset()
        {
            AnnotationFile.Delete();
        }
    }
}
