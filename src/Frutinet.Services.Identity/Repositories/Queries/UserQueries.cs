using System;
using System.Threading.Tasks;
using Frutinet.Common.Extensions;
using Frutinet.Common.Mongo;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Frutinet.Services.Identity.Users.Repositories.Queries
{
    public static class UserQueries
    {
        public static IMongoCollection<User> Users(this IMongoDatabase database)
            => database.GetCollection<User>();

        public static async Task<bool> ExistsAsync(this IMongoCollection<User> users, string name)
            => await users.AsQueryable().AnyAsync(x => x.UserName == name);

        public static async Task<User> GetOwnerAsync(this IMongoCollection<User> users)
            => await users.AsQueryable().FirstOrDefaultAsync(x => x.Role == Role.Owner);

        public static async Task<User> GetByUserIdAsync(this IMongoCollection<User> users, Guid userId)
        {
            if (userId == null)
                return null;

            return await users.AsQueryable().FirstOrDefaultAsync(x => x.Id == userId);
        }

        public static async Task<User> GetByExternalUserIdAsync(this IMongoCollection<User> users, string externalUserId)
        {
            if (externalUserId.Empty())
                return null;

            return await users.AsQueryable().FirstOrDefaultAsync(x => x.ExternalUserId == externalUserId);
        }

        public static async Task<User> GetByEmailAsync(this IMongoCollection<User> users, string email)
        {
            if (email.Empty())
                return null;

            return await users.AsQueryable().FirstOrDefaultAsync(x => x.Email == email);
        }

        public static async Task<User> GetByNameAsync(this IMongoCollection<User> users, string name)
        {
            if (name.Empty())
                return null;

            return await users.AsQueryable().FirstOrDefaultAsync(x => x.UserName == name);
        }

        public static async Task<State?> GetStateAsync(this IMongoCollection<User> users, Guid id)
        {
            if (id == null)
                return null;

            return await users.AsQueryable().Where(x => x.Id == id)
                .Select(x => x.State)
                .FirstOrDefaultAsync();
        }

        //public static IMongoQueryable<User> Query(this IMongoCollection<User> users,
        //    BrowseUsers query)
        //{
        //    var values = users.AsQueryable();

        //    return values.OrderBy(x => x.Name);
        //}
    }
}