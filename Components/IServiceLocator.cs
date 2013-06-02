using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components
{
    public interface IServiceLocator
    {
        bool IsRegistered<TService>();
        void Register<TService>(TService service);
        TService Resolve<TService>();
        TService ResolveOrCreate<TService>(Func<TService> create);
        TService ResolveOrCreate<TService>() where TService : new();
    }
}
