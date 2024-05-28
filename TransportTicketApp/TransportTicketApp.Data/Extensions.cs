namespace TransportTicketApp.Data
{
    public static class Extensions
    {
        public static EntityType SetIdentityParams<EntityType, DtoType>(this EntityType entity, DtoType dto)
            where EntityType : IEntityBase
            where DtoType : IDtoForUpdateOrDelete
        {
            entity.Id = dto.Id;
            //entity.GUID = dto.GUID;
            return entity;
        }

        public static EntityType SetIdentityParams<EntityType>(this EntityType entity, EntityType master)
             where EntityType : IEntityBase
        {
            entity.Id = master.Id;
            //entity.GUID = master.GUID;
            //entity.CreateTime = master.CreateTime;
            //entity.UpdateTime = master.UpdateTime;
            return entity;
        }
#if false
        public static List<NestedEntityType> ToEntityList<EntityType, NestedEntityType, InsertDtoType>(this IEnumerable<InsertDtoType> dtosForAdd
              , EntityType parentObject
              , IDtoEntityConverterForInsert<InsertDtoType, NestedEntityType> insertConverter)
              where EntityType : IUniqueIdentityEntity
              where NestedEntityType : IUniqueIdentityEntity
              where InsertDtoType : IDtoForInsert
        {
            return new List<NestedEntityType>(dtosForAdd.Select(x => insertConverter.Convert(x, parentObject)));
        }
        public static EntitySet<NestedEntityType, ComparerType> ToEntitySet<EntityType, NestedEntityType, InsertDtoType, ComparerType>(this IEnumerable<InsertDtoType> dtosForAdd
               , ComparerType comparerType
               , EntityType parentObject
               , IDtoEntityConverterForInsert<InsertDtoType, NestedEntityType> insertConverter)
               where EntityType : IUniqueIdentityEntity
               where NestedEntityType : IUniqueIdentityEntity
               where InsertDtoType : IDtoForInsert
               where ComparerType : IEqualityComparer<NestedEntityType>, new()
        {
            return new EntitySet<NestedEntityType, ComparerType>(dtosForAdd.Select(x => insertConverter.Convert(x, parentObject)));

        }


        public static List<NestedEntityType>? UpdatePackConverter<EntityType, InsertDtoType, UpdateDtoType, DeleteDtoType, NestedEntityType>
               (this IUpdatePack<InsertDtoType, UpdateDtoType, DeleteDtoType>? pack
               , EntityType parent
               , List<NestedEntityType>? nestedItemList
               , IDtoEntityConverterForInsert<InsertDtoType, NestedEntityType> insertConverter
               , IDtoEntityConverterForUpdate<UpdateDtoType, NestedEntityType> updateConverter
               , IDtoEntityConverterForDelete<DeleteDtoType, NestedEntityType> deleteConverter
               ) where EntityType : IUniqueIdentityEntity
               where NestedEntityType : IUniqueIdentityEntity
               where UpdateDtoType : IDtoForUpdateOrDelete
               where DeleteDtoType : IDtoForUpdateOrDelete
               where InsertDtoType : IDtoForInsert
        {
            if (pack is null)
                return nestedItemList;

            List<NestedEntityType> result = nestedItemList is null
                                          ? new List<NestedEntityType>()
                                          : new List<NestedEntityType>(nestedItemList);

            foreach (var item in pack.Deleted)
            {
                NestedEntityType l = deleteConverter.Convert(item);
                result.RemoveAll(x => x.Id == l.Id);
            }

            foreach (var item in pack.Updated)
            {
                NestedEntityType? originalOption = result.FirstOrDefault(x => x.Id == item.Id);
                if (originalOption is not null)
                {
                    NestedEntityType l = updateConverter.Convert(originalOption!, item);
#warning Replace için daha mantıklı bişi yapılabilir.
                    result.RemoveAll(x => x.Id == l.Id);
                    result.Add(l);
                }
            }

            foreach (var item in pack.Added)
            {
                NestedEntityType l = insertConverter.Convert(item, parent);
                result.Add(l);
            }
            return result;
        }



        public static EntitySet<NestedEntityType, ComparerType>? UpdatePackConverterEntity<EntityType, InsertDtoType, UpdateDtoType, DeleteDtoType, NestedEntityType, ComparerType>
               (this IUpdatePack<InsertDtoType, UpdateDtoType, DeleteDtoType>? pack
               , EntityType parent
               , EntitySet<NestedEntityType, ComparerType>? nestedItemList
               , IDtoEntityConverterForInsert<InsertDtoType, NestedEntityType> insertConverter
               , IDtoEntityConverterForUpdate<UpdateDtoType, NestedEntityType> updateConverter
               , IDtoEntityConverterForDelete<DeleteDtoType, NestedEntityType> deleteConverter
               )
               where EntityType : IUniqueIdentityEntity
               where NestedEntityType : IUniqueIdentityEntity
               where UpdateDtoType : IDtoForUpdateOrDelete
               where DeleteDtoType : IDtoForUpdateOrDelete
               where InsertDtoType : IDtoForInsert
               where ComparerType : IEqualityComparer<NestedEntityType>, new()
        {
            if (pack is null)
                return nestedItemList;

            EntitySet<NestedEntityType, ComparerType> result = nestedItemList is null
                                          ? new EntitySet<NestedEntityType, ComparerType>()
                                          : new EntitySet<NestedEntityType, ComparerType>(nestedItemList);

            foreach (var item in pack.Deleted)
            {
                NestedEntityType l = deleteConverter.Convert(item);
                result.RemoveWhere(x => x.Id == l.Id);
                //result.RemoveAll(x => x.Id == l.Id);
            }

            foreach (var item in pack.Updated)
            {

                NestedEntityType? originalOption = result.FirstOrDefault(x => x.Id == item.Id);
                if (originalOption is not null)
                {
                    NestedEntityType l = updateConverter.Convert(originalOption!, item);
#warning Replace için daha mantıklı bişi yapılabilir.
                    result.RemoveWhere(x => x.Id == l.Id);
                    result.Add(l);
                }
            }

            foreach (var item in pack.Added)
            {
                NestedEntityType l = insertConverter.Convert(item, parent);
                result.Add(l);
            }
            return result;
        }


        // Emre abinin son yazdığı kodlar

        public static EntityType? Convert<EntityType, DtoType>(this DtoType? dto, IDtoEntityConverterForInsert<DtoType, EntityType>? converter, IUniqueIdentityEntity parent)
            where EntityType : IUniqueIdentityEntity, new()
        {
            return converter is null || dto is null ? default : converter.Convert(dto!, parent);
        }
        public static EntityType? Convert<EntityType, DtoType>(this DtoType? dto, EntityType originalData, IDtoEntityConverterForUpdate<DtoType, EntityType>? converter)
            where EntityType : IUniqueIdentityEntity, new()
            where DtoType : IDtoForUpdateOrDelete
        {
            return converter is null || dto is null ? default : converter.Convert(originalData, dto!);
        }
        public static EntityType? Convert<EntityType, DtoType>(this DtoType? dto, IDtoEntityConverterForDelete<DtoType, EntityType>? converter)
            where EntityType : IUniqueIdentityEntity, new()
            where DtoType : IDtoForUpdateOrDelete
        {
            return converter is null || dto is null ? default : converter.Convert(dto!);
        }
        //public static EntityType? Clone<EntityType>(this EntityType? dto, IEntityCloner<EntityType>? cloner)
        //   where EntityType : IUniqueIdentityEntity, new()
        //{
        //    return cloner is null || dto is null ? default : cloner.Clone(dto!);
        //}

        public static IEnumerable<EntityType>? Convert<EntityType, DtoType>(this IEnumerable<(DtoType dto, IUniqueIdentityEntity parent)>? dtos, IDtoEntityConverterForInsert<DtoType, EntityType>? converter)
    where EntityType : IUniqueIdentityEntity, new()
        {
            if (!(converter is null || dtos is null))
                foreach (var item in dtos)
                    yield return converter.Convert(item.dto, item.parent);
        }
        public static IEnumerable<EntityType>? Convert<EntityType, DtoType>(this IEnumerable<(DtoType dto, EntityType originalData)>? dtos, IDtoEntityConverterForUpdate<DtoType, EntityType>? converter)
            where EntityType : IUniqueIdentityEntity, new()
            where DtoType : IDtoForUpdateOrDelete
        {
            if (!(converter is null || dtos is null))
                foreach (var item in dtos)
                    yield return converter.Convert(item.originalData, item.dto);
        }
        public static IEnumerable<EntityType>? Convert<EntityType, DtoType>(this IEnumerable<DtoType>? dto, IDtoEntityConverterForDelete<DtoType, EntityType>? converter)
            where EntityType : IUniqueIdentityEntity, new()
            where DtoType : IDtoForUpdateOrDelete
        {
            if (!(converter is null || dto is null))
                foreach (var item in dto)
                    yield return converter.Convert(item);
        }
        public static IEnumerable<EntityType>? Clone<EntityType>(this IEnumerable<EntityType>? dto, IEntityCloner<EntityType>? cloner)
          where EntityType : IUniqueIdentityEntity, new()
        {
            if (!(cloner is null || dto is null))
                foreach (var item in dto)
                    yield return cloner.Clone(item);
        }
        public static IEnumerable<EntityType>? Convert<EntityType, DtoType>(this IEnumerable<DtoType>? dtos, IDtoEntityConverterForInsert<DtoType, EntityType>? converter)
    where EntityType : IUniqueIdentityEntity, new()
        {
            if (!(converter is null || dtos is null))
                foreach (var item in dtos)
                    yield return converter.Convert(item, null);
        }


        public static IEntitySet<EntityType, ComparerType>? Convert<EntityType, DtoType, ComparerType>(this IEnumerable<(DtoType dto, IUniqueIdentityEntity parent)>? dtos, IDtoEntityConverterForInsert<DtoType, EntityType>? converter)
            where EntityType : IUniqueIdentityEntity, new()
            where ComparerType : IEqualityComparer<EntityType>, new()
        {
            return new EntitySet<EntityType, ComparerType>(dtos.Convert(converter));
        }
        public static IEntitySet<EntityType, ComparerType>? Convert<EntityType, DtoType, ComparerType>(this IEnumerable<(DtoType dto, EntityType originalData)>? dtos, IDtoEntityConverterForUpdate<DtoType, EntityType>? converter)
            where EntityType : IUniqueIdentityEntity, new()
            where DtoType : IDtoForUpdateOrDelete
            where ComparerType : IEqualityComparer<EntityType>, new()
        {
            return new EntitySet<EntityType, ComparerType>(dtos.Convert(converter));
        }
        public static IEntitySet<EntityType, ComparerType>? Convert<EntityType, DtoType, ComparerType>(this IEnumerable<DtoType>? dto, IDtoEntityConverterForDelete<DtoType, EntityType>? converter)
            where EntityType : IUniqueIdentityEntity, new()
            where DtoType : IDtoForUpdateOrDelete
            where ComparerType : IEqualityComparer<EntityType>, new()
        {
            return new EntitySet<EntityType, ComparerType>(dto.Convert(converter));
        }
        public static IEntitySet<EntityType, ComparerType>? Clone<EntityType, ComparerType>(this IEnumerable<EntityType>? dto, IEntityCloner<EntityType>? cloner)
            where EntityType : IUniqueIdentityEntity, new()
            where ComparerType : IEqualityComparer<EntityType>, new()
        {
            return new EntitySet<EntityType, ComparerType>(dto.Clone(cloner));
        }

        public static EntitySet<EntityType, ComparerType>? ToSet<EntityType, ComparerType>(this IEntitySet<EntityType, ComparerType>? q)
            where EntityType : IUniqueIdentityEntity, new()
            where ComparerType : IEqualityComparer<EntityType>, new()
        {
            if (q is EntitySet<EntityType, ComparerType> w)
                return w;
            else
                return new EntitySet<EntityType, ComparerType>(q);
        }
#endif 
    }
}

