using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components
{
    public class ServiceLocator : IServiceLocator
    {
        private static ServiceLocator _default = new ServiceLocator();

        public static ServiceLocator Default
        {
            get { return _default; }
        }

        private Dictionary<Type, object> _services = new Dictionary<Type, object>();

        public bool IsRegistered<TService>()
        {
            lock (_services)
            {
                return _services.ContainsKey(typeof(TService));
            }
        }

        public void Register<TService>(TService service)
        {
            lock (_services)
            {
                if (IsRegistered<TService>())
                {
                    throw new InvalidOperationException();
                }
                else
                {
                    _services.Add(typeof(TService), service);
                }
            }
        }

        public TService Resolve<TService>()
        {
            lock (_services)
            {
                object service;
                var success = _services.TryGetValue(typeof(TService), out service);
                return success ? (TService)service : default(TService);
            }
        }

        public TService ResolveOrCreate<TService>(Func<TService> create)
        {
            lock (_services)
            {
                object service;
                var success = _services.TryGetValue(typeof(TService), out service);

                if (success)
                {
                    return (TService)service;
                }
                else
                {
                    var s = create();
                    _services.Add(typeof(TService), s);
                    return s;
                }
            }
        }

        public TService ResolveOrCreate<TService>()
            where TService : new()
        {
            return ResolveOrCreate(() => new TService());
        }
    }
}
