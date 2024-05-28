namespace TransportTicketApp.Data
{
   
    public interface IEntityBase 
    {
        Guid Id { get; set; }
    }
    public interface IIntKeyEntity 
    {
    }
    //public static class Extensions2
    //{
    //    public static T GetId<T>(this IEntityBase<T> entity) => entity.Id;
    //}


}
