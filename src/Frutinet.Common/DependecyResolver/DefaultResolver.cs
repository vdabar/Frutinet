using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frutinet.Common.DependecyResolver
{
    public class DefaultResolver : IResolver
    {
        private readonly IWebHost _webHost;

        public DefaultResolver(IWebHost webHost)
        {
            _webHost = webHost;
        }

        public T Resolve<T>() => (T)_webHost.Services.GetService(typeof(T));
    }
}