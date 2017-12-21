using Frutinet.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frutinet.Common.Domain
{
    public interface IAggregateRoot
    {
        Guid Id { get; }
        ICollection<IEvent> Events { get; }
    }
}