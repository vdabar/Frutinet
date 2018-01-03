using Frutinet.Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frutinet.Services.Identity.Users.Rules
{
    public interface IUserRules : IRules<User>
    {
        Task<bool> IsUserNameUnique(string name, Guid userId = new Guid());

        Task<bool> IsUserEmailUnique(string email, Guid userId = new Guid());
    }
}