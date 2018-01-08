using Frutinet.Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frutinet.Services.Identity.Users.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmailAsync(string email);

        Task<User> GetByUserIdAsync(Guid userId);

        Task AddAsync(User user);

        Task UpdateAsync(User user);

        Task DeleteAsync(Guid userId);

        Task<bool> ExistsAsync(string name);

        Task<User> GetByNameAsync(string name);
    }
}