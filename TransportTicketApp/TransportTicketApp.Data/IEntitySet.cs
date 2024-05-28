
namespace TransportTicketApp.Data
{
    public interface IEntitySet<EntityType, ComparerType> : ISet<EntityType> where EntityType : IEntityBase
                                                                         where ComparerType : IEqualityComparer<EntityType>, new()
    { }
    public class EntitySet<EntityType, ComparerType> : HashSet<EntityType>, IEntitySet<EntityType, ComparerType> where EntityType : IEntityBase
                                                                                                                 where ComparerType : IEqualityComparer<EntityType> , new()
    {
        public EntitySet() : base(new ComparerType())
        {

        }
        public EntitySet(IEnumerable<EntityType> collection) : base(collection ?? Enumerable.Empty<EntityType>(), new ComparerType())
        {

        }
    }
    public static class X
    {
        public static IEntitySet<EntityType1, ComparerType> To<EntityType1, EntityType2, ComparerType>(this EntitySet<EntityType2, ComparerType> from)
            where EntityType1 : IEntityBase
            where EntityType2 : class, IEntityBase, EntityType1
            where ComparerType : IEqualityComparer<EntityType1>, new()
        {
            return new EntitySet<EntityType1, ComparerType>(from);
        }
        public static EntitySet<EntityType2, ComparerType> To2<EntityType1, EntityType2, ComparerType>(this IEntitySet<EntityType1, ComparerType> from)
            where EntityType1 : IEntityBase
            where EntityType2 : class, IEntityBase, EntityType1
            where ComparerType : IEqualityComparer<EntityType1>, new()
        {
            if (from is EntitySet<EntityType2, ComparerType> q)
                return q;
            else
                return new EntitySet<EntityType2, ComparerType>(from.OfType<EntityType2>());
        }
    }
}
