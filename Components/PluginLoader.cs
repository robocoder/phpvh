using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Components
{
    public class PluginLoader
    {
        public IEnumerable<TPlugin> Load<TPlugin>(Assembly asm)
        {
            return asm
                .GetTypes()
                .Where(x => x.IsDerivedFromOrImplements<TPlugin>())
                .Select(Activator.CreateInstance)
                .Cast<TPlugin>();
        }

        public IEnumerable<TPlugin> Load<TPlugin>()
        {
            return Load<TPlugin>(Assembly.GetCallingAssembly());
        }
    }
}
