using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frutinet.Common.DependecyResolver
{
    public class AutofacResolver : IResolver
    {
        private readonly ILifetimeScope _scope;

        public AutofacResolver(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public T Resolve<T>() => _scope.Resolve<T>();
    }
}