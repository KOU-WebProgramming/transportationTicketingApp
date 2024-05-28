namespace TransportTicketApp.Data
{
    public interface IDtoForInsert { }
    public interface IDtoForUpdateOrDelete
    {
        Guid Id { get; set; }
    }

    public interface IDtoEntityConverterForInsert<in DtoType, out EntityType> where EntityType : IEntityBase
    {
        EntityType Convert(DtoType dto, IEntityBase? parent = null);
    }
    public interface IDtoEntityConverterForUpdate<in DtoType, EntityType> where EntityType : IEntityBase where DtoType : IDtoForUpdateOrDelete
    {
        EntityType Convert(EntityType originalData, DtoType dto);
    }
    public interface IDtoEntityConverterForDelete<in DtoType, out EntityType> where EntityType : IEntityBase where DtoType : IDtoForUpdateOrDelete
    {
        EntityType Convert(DtoType dto);
    }

    public interface IEntityCloner<EntityType> where EntityType : IEntityBase 
    {
        EntityType Clone(EntityType dto);
    }

    public interface IUpdatePack<InsertDtoType, UpdateDtoType, DeleteDtoType> where InsertDtoType : IDtoForInsert
                                                                             where UpdateDtoType : IDtoForUpdateOrDelete
                                                                             where DeleteDtoType : IDtoForUpdateOrDelete
    {
        List<InsertDtoType> Added   { get; set; }
        List<UpdateDtoType> Updated { get; set; }
        List<DeleteDtoType> Deleted { get; set; }
    }
}
