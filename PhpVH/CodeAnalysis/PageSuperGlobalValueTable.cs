using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Components;

namespace PhpVH.CodeAnalysis
{
    public class PageSuperGlobalValueTable : TableBase<string, SuperGlobalValueTable>
    {
        public override List<SuperGlobalValueTable> Items { get; protected set; }

        public PageSuperGlobalValueTable()
        {
            Items = new List<SuperGlobalValueTable>();
        }

        protected override string GetKey(SuperGlobalValueTable element)
        {
            return element.Filename;
        }

        protected override SuperGlobalValueTable CreateElement(string key)
        {
            return new SuperGlobalValueTable(key);
        }
    }
}
