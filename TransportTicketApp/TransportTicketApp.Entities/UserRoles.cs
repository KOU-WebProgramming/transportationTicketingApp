using MongoDbGenericRepository.Attributes;
using TransportTicketApp.Data.Mongo.Configurations;

namespace TransportTicketApp.Entities
{
    [MongoEntity("UserRoles")]
    [CollectionName("UserRoles")]
    public static class UserRoles
    {
        public const string Admin = "Admin";
        public const string Customer = "Customer";
    }
}
