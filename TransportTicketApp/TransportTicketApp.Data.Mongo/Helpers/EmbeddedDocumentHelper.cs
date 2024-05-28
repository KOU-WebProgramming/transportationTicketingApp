namespace TransportTicketApp.Data.Mongo.Helpers
{
    public static class EmbeddedDocumentHelper
    {
        //public static EntityType SetCreateParams<EntityType>() where EntityType : IEntityBase, new()
        //{
        //    return new EntityType
        //    {
        //        Id = ObjectId.GenerateNewId().ToString(),
        //        GUID = Guid.NewGuid().ToString(),
        //        CreateTime = IDateTimeProvider.GetDefaultProvider.UtcNow,
        //    };
        //}

        public static EntityType SetCreateParams<EntityType>(this EntityType entityType, IEntityBase? parent = null) where EntityType : IEntityBase, new()
        {
            //entityType.Id           = ObjectId.GenerateNewId().ToString();
            entityType.Id = Guid.Parse(string.Concat(parent is null ? "" : $"{parent.Id}:", Guid.NewGuid().ToString()));
            //entityType.CreateTime = DateTime.UtcNow;

            return entityType;
        }
        public static EntityType SetCreateParamsWithGuid<EntityType>(this EntityType entityType, string guid, IEntityBase? parent = null) where EntityType : IEntityBase, new()
        {
            //entityType.Id           = ObjectId.GenerateNewId().ToString();
            entityType.Id = Guid.Parse(string.Concat(parent is null ? "" : $"{parent.Id}:", guid));
            //entityType.CreateTime = DateTime.UtcNow;

            return entityType;
        }
    }
}
