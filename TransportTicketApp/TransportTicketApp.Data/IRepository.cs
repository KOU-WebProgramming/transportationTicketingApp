namespace TransportTicketApp.Data
{
    using System.Linq.Expressions;

    namespace IMS.Framework.Data
    {
        public interface IRepository<EntityType> where EntityType : IEntityBase
    {
        Task<IEnumerable<EntityType>>                           GetAll          ();
        Task<IEnumerable<EntityType>>                           Get             (Expression<Func<EntityType, bool>> filter);
        Task<EntityType?>                                       Create          (EntityType item);
        Task<IEnumerable<EntityType>?>                          CreateMany      (IEnumerable<EntityType> items);
        Task<bool>                                              Update          (EntityType item);
        Task<bool>                                              UpdateMany      (IEnumerable<EntityType> items);
        Task                                                    Delete          (EntityType item);
        Task                                                    DeleteMany      (IEnumerable<EntityType> items);
        Task<EntityType?>                                       Get             (string id);
        Task<bool>                                              Update          (string id, EntityType item);
        Task                                                    Delete          (string id);
        Task                                                    DeleteGuid      (string guid);
        Task                                                    DeleteMany      (IEnumerable<string> items);
        IQueryable<EntityType>                                  GetAsQueryable  (Expression<Func<EntityType, bool>>? filter = null);

        Task<EmbeddedDocumentType> GetEmbeddedDocument<EmbeddedDocumentType>(Expression<Func<EntityType
                                                                           , IEnumerable<EmbeddedDocumentType>>> embeddedSelector
                                                                           , Expression<Func<EmbeddedDocumentType, bool>> filter);

        Task<EntityType> UpdateEmbeddedDocumentField<EmbeddedDocumentType>(Expression<Func<EntityType, IEnumerable<EmbeddedDocumentType>>> embeddedSelector
                                                                              , Expression<Func<EmbeddedDocumentType, bool>> filter
                                                                              , Expression<Func<EmbeddedDocumentType, object>> x
                                                                              , object value) where EmbeddedDocumentType : IEntityBase;

        Task<EntityType> UpdateEmbeddedDocument<EmbeddedDocumentType>(Expression<Func<EntityType, IEnumerable<EmbeddedDocumentType>>> embeddedSelector
                                                                      , Expression<Func<EmbeddedDocumentType, bool>> filter
                                                                      , EmbeddedDocumentType value) where EmbeddedDocumentType : IEntityBase;


        Task DeleteEmbeddedDocument<EmbeddedDocumentType>(Expression<Func<EntityType, IEnumerable<EmbeddedDocumentType>>> embeddedSelector
                                                        , Expression<Func<EmbeddedDocumentType, bool>> filter);

        Task<IEnumerable<EntityType>?> GetAllPagination(int pageNumber, int pageOffset);
        Task<IEnumerable<EntityType>?> GetAllPagination(Expression<Func<EntityType, bool>> filter, int pageNumber, int pageOffset);


    }
}

}
