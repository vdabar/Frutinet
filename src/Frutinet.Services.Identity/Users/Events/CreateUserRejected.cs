using Frutinet.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frutinet.Services.Identity.Users.Events
{
    public class CreateUserRejected : IRejectedEvent
    {
        public string Reason { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
    }
}