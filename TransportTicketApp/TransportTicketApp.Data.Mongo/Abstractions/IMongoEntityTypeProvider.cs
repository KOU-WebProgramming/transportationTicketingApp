using System.Reflection;

namespace TransportTicketApp.Data.Mongo.Abstractions
{
    public interface IMongoEntityTypeProviderAssemblyLoader
    {
        Assembly? Load();
    }
    public interface IMongoEntityTypeProvider
    {
        public IReadOnlyDictionary<Type, string> GetEntityTypeTypes();
    }
}
