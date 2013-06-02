using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhpVH.ScanPlugins;

namespace PhpVH
{
    public class TraceTable : Dictionary<ScanPluginBase, Dictionary<int, Dictionary<string, FileTrace>>>
    {
    }
}
