using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TransportTicketApp.Data.Mongo.Abstractions;
using TransportTicketApp.Data.Mongo.Configurations;
using TransportTicketApp.Data.Mongo.Implementations;
namespace TransportTicketApp.Data.Mongo.Extensions
{
    public static class MongoDbExtensions
    {

        public static IServiceCollection AddMongoDbContext<ProviderType>(this IServiceCollection services, IConfiguration configuration, Type entityType) where ProviderType : class, IMongoEntityTypeProvider
        {
            return AddMongoDbContext<ProviderType>(services, configuration, new MongoEntityTypeProviderFromEntityType(entityType));
        }

        public static IServiceCollection AddMongoDbContext<ProviderType>(this IServiceCollection services, IConfiguration configuration, IMongoEntityTypeProviderAssemblyLoader assemblyLoader) where ProviderType : class, IMongoEntityTypeProvider
        {
            services.AddSingleton<IMongoEntityTypeProviderAssemblyLoader>(assemblyLoader);
            services.AddSingleton<IMongoEntityTypeProvider, ProviderType>();
            services.Configure<MongoDbOptions>(o => configuration.GetSection(MongoDbOptions.SectionName).Bind(o));
            services.AddScoped<IDbContext, MongoDbContext>();
            return services;
        }

        public static IServiceCollection AddMongoDbContext<AssemblyProvider, ProviderType>(this IServiceCollection services, IConfiguration configuration) where ProviderType : class, IMongoEntityTypeProvider
                                                                                                                                                           where AssemblyProvider : class, IMongoEntityTypeProviderAssemblyLoader
        {
            services.AddSingleton<IMongoEntityTypeProviderAssemblyLoader, AssemblyProvider>();
            services.AddSingleton<IMongoEntityTypeProvider, ProviderType>();
            services.Configure<MongoDbOptions>(o => configuration.GetSection(MongoDbOptions.SectionName).Bind(o));
            services.AddScoped<IDbContext, MongoDbContext>();
            return services;
        }

    }
    public class MongoEntityTypeProviderExecutingAssemblyLoader : IMongoEntityTypeProviderAssemblyLoader
    {
        public Assembly? Load() => Assembly.GetExecutingAssembly();
    }

    public class MongoEntityTypeProviderFromEntityType : IMongoEntityTypeProviderAssemblyLoader
    {
        private readonly Type entityType;

        public MongoEntityTypeProviderFromEntityType(Type entityType)
        {
            this.entityType = entityType;
        }
        public Assembly? Load() => entityType.Assembly;
    }
}
