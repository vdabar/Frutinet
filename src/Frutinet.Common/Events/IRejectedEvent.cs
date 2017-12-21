using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frutinet.Common.Events
{
    public interface IRejectedEvent : IEvent
    {
        string Reason { get; set; }
        string Code { get; set; }
    }
}