using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Humanizer;
using System;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Linq;

namespace Frutinet.Common.Mongo
{
    public static class Extensions
    {
        public static void AddMongoDB(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoOptions>(options => configuration.GetSection("mongoDb").Bind(options));
            services.AddSingleton<MongoClient>(c =>
            {
                var options = c.GetService<IOptions<MongoOptions>>();
                return new MongoClient(options.Value.ConnectionString);
            });
            services.AddScoped<IMongoDatabase>(c =>
            {
                var options = c.GetService<IOptions<MongoOptions>>();
                var client = c.GetService<MongoClient>();
                return client.GetDatabase(options.Value.Database);
            });
            services.AddScoped<IDatabaseInitializer, MongoInitializer>();
            services.AddScoped<IDatabaseSeeder, MongoSeeder>();
        }

        public static IMongoCollection<T> GetCollection<T>(this IMongoDatabase database)
        {
            var collectionName = Pluralize<T>();
            var collection = database.GetCollection<T>(collectionName);

            return collection;
        }

        private static string Pluralize<T>() => Pluralize(typeof(T));

        private static string Pluralize(Type type)
        {
            var typeName = type.Name;
            if (typeName.EndsWith("Dto"))
                typeName = typeName.Replace("Dto", string.Empty);

            var pluralizedName = typeName.Pluralize();

            return pluralizedName;
        }

        public static async Task<IList<T>> GetAllAsync<T>(this IMongoCollection<T> collection)
            => await GetAllAsync(collection, _ => true);

        public static async Task<IList<T>> GetAllAsync<T>(this IMongoCollection<T> collection,
            Expression<Func<T, bool>> filter)
        {
            var filteredCollection = await collection.FindAsync(filter);
            var entities = await filteredCollection.ToListAsync();

            return entities;
        }

        public static async Task<T> FirstOrDefaultAsync<T>(this IMongoCollection<T> collection,
            Expression<Func<T, bool>> filter)
        {
            var filteredCollection = await collection.FindAsync(filter);
            var entities = await filteredCollection.ToListAsync();
            var entity = entities.FirstOrDefault();

            return entity;
        }

        public static async Task CreateCollectionAsync<T>(this IMongoDatabase database)
            => await database.CreateCollectionAsync(Pluralize<T>());

        public static async Task DropCollectionAsync<T>(this IMongoDatabase database)
            => await database.DropCollectionAsync(Pluralize<T>());
    }
}