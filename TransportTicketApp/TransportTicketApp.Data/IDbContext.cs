using TransportTicketApp.Data.IMS.Framework.Data;

namespace TransportTicketApp.Data
{
    public interface IDbContext
    {
        IRepository<EntityType>? GetRepository<EntityType>() where EntityType : IEntityBase;
    }
}
