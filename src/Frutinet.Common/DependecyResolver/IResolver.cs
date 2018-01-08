using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frutinet.Common.DependecyResolver
{
    public interface IResolver
    {
        T Resolve<T>();
    }
}