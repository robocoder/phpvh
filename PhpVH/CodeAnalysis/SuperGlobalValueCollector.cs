using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Components;
using PhpVH.LexicalAnalysis;

namespace PhpVH.CodeAnalysis
{
    public class SuperGlobalValueCollector
    {
        public static SuperGlobalValueCollector Current = new SuperGlobalValueCollector();

        public PageSuperGlobalValueTable PageTable { get; private set; }

        public SuperGlobalValueCollector()
        {
            PageTable = new PageSuperGlobalValueTable();
        }

        public void AddValues(string filename, PhpToken[] tokens)
        {
            var walker = new ExpressionWalker(
                tokens,
                new BooleanExpressionAnalyzer(),
                new SwitchStatementAnalyzer());

            var valueTable = new SuperGlobalValueTable(filename);

            walker.Walk();
            walker.Analyzers.Iter(x =>
            {
                x.Matches.Iter(y =>
                {
                    var id = y.Tokens[0].Lexeme;
                    var list = valueTable[id];

                    if (list == null)
                    {
                        list = new SuperGlobalValueList(id);
                        valueTable.Add(list);
                    }
                    
                    Func<string, string> s = a => a.Substring(1, a.Length - 2);

                    if (x is BooleanExpressionAnalyzer)
                    {
                        list.Values.Add(
                            new SuperGlobalNameValuePair(
                                s(y.Tokens[1].Lexeme),
                                s(y.Tokens[3].Lexeme)));
                    }
                    else if (x is SwitchStatementAnalyzer)
                    {
                        list.Values.AddRange(
                            y.Tokens
                                .Skip(2)
                                .Select(z => new SuperGlobalNameValuePair(
                                    s(y.Tokens[1].Lexeme),
                                    s(z.Lexeme))));
                    }
                });
            });

            lock (PageTable)
            {
                PageTable.Add(valueTable);
            }
        }        
    }
}
