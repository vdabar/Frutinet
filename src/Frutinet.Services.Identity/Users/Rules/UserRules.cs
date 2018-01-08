using Frutinet.Services.Identity.Users.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frutinet.Services.Identity.Users.Rules
{
    public class UserRules : IUserRules
    {
        private readonly IUserRepository _userRepository;

        public UserRules(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> IsUserNameUnique(string name, Guid userId = new Guid())
        {
            var user = await _userRepository.GetByNameAsync(name);
            return true;
        }

        public async Task<bool> IsUserEmailUnique(string email, Guid userId = new Guid())
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return true;
        }
    }
}