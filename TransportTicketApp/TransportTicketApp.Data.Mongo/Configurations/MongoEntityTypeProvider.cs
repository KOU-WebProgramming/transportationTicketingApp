using System.Collections.ObjectModel;
using System.Reflection;
using TransportTicketApp.Data.Mongo.Abstractions;

namespace TransportTicketApp.Data.Mongo.Configurations
{
    public class MongoEntityTypeProvider : IMongoEntityTypeProvider
    {
        private readonly IReadOnlyDictionary<Type, string> _typeCollectionTable = null!;
        public                  /*Ctor*/                            MongoEntityTypeProvider(IMongoEntityTypeProviderAssemblyLoader assemblyLoader)
        {
            Dictionary<Type, string> typeCollectionTable = new Dictionary<Type, string>();
            Assembly? assembly = assemblyLoader.Load();

            if (assembly is not null)
                foreach (Type item in assembly.GetTypes())
                    if (item.GetCustomAttribute<MongoEntityAttribute>() is MongoEntityAttribute attr)
                        typeCollectionTable[item] = attr.CollectionName;

            _typeCollectionTable = new ReadOnlyDictionary<Type, string>(typeCollectionTable);
        }
        public IReadOnlyDictionary<Type, string> GetEntityTypeTypes()
        {
            return _typeCollectionTable!;
        }
    }
}
