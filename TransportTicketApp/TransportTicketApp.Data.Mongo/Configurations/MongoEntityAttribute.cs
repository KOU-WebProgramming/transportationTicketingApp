namespace TransportTicketApp.Data.Mongo.Configurations
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class MongoEntityAttribute : Attribute
    {
        public string CollectionName { get; }
        public  /*Ctor*/    MongoEntityAttribute(string collectionName)
        {
            CollectionName = collectionName;
        }
    }
}
