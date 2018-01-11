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
        public Guid RequestId { get; set; }
        public string UserId { get; set; }
        public string Code { get; set; }
        public string Reason { get; set; }

        public CreateUserRejected()
        {
        }

        public CreateUserRejected(Guid requestId, string userId, string code, string reason)
        {
            RequestId = requestId;
            UserId = userId;
            Code = code;
            Reason = reason;
        }
    }
}