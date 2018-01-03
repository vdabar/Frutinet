using Frutinet.Services.Identity.Users.Repositories.Queries;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frutinet.Services.Identity.Users.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoDatabase _database;

        public UserRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<bool> ExistsAsync(string name)
            => await _database.Users().ExistsAsync(name);

        public async Task<User> GetOwnerAsync()
            => await _database.Users().GetOwnerAsync();

        public async Task<User> GetByUserIdAsync(string userId)
            => await _database.Users().GetByUserIdAsync(userId);

        public async Task<User> GetByExternalUserIdAsync(string externalUserId)
            => await _database.Users().GetByExternalUserIdAsync(externalUserId);

        public async Task<User> GetByEmailAsync(string email, string provider)
            => await _database.Users().GetByEmailAsync(email, provider);

        public async Task<User> GetByNameAsync(string name)
            => await _database.Users().GetByNameAsync(name);

        public async Task<string> GetStateAsync(string id)
            => await _database.Users().GetStateAsync(id);

        //public async Task<User> BrowseAsync(BrowseUsers query)
        //{
        //    return await _database.Users()
        //        .Query(query)
        //        .PaginateAsync(query);
        //}

        public async Task AddAsync(User user)
            => await _database.Users().InsertOneAsync(user);

        public async Task UpdateAsync(User user)
            => await _database.Users().ReplaceOneAsync(x => x.Id == user.Id, user);

        public async Task DeleteAsync(string userId)
            => await _database.Users().DeleteOneAsync(x => x.UserId == userId);
    }
}