using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace PhpVH.ScanPlugins
{
    public static class IEnumerableSqlFunctionExtension
    {
        public static Hook[] ToHooks(this IEnumerable<SqlFunction> functions)
        {
            return functions.Select(x => new Hook(x.Name, x.ParamCount)).ToArray();
        }
    }
}
