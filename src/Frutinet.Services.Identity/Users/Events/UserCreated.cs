using Frutinet.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frutinet.Services.Identity.Users.Events
{
    public class UserCreated : IEvent
    {
        public string Email { get; set; }
        public string Name { get; set; }
    }
}