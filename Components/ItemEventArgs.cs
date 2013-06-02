using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    public class ItemEventArgs<TItem> : EventArgs
    {
        public TItem Item { get; set; }

        public ItemEventArgs(TItem item)
        {
            Item = item;
        }

        public ItemEventArgs() { }
    }
}
