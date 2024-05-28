using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;
using TransportTicketApp.Data.Mongo.Configurations;

namespace TransportTicketApp.Entities
{
    [CollectionName("Users")]
    [MongoEntity("Users")]

    public class ApplicationUser : MongoIdentityUser<Guid>
    {
        public string       FirstName               { get; set; }
        public string       LastName                { get; set; }
        public List<string> RoleIds { get; set; }
    }


    [CollectionName("Roles")]
    [MongoEntity("Roles")]

    public class ApplicationRole : MongoIdentityRole<Guid>
    {
        public ApplicationRole() : base() { }

        public ApplicationRole(string roleName) : base(roleName)
        {
        }
    }

}
