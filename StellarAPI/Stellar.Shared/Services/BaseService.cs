using Stellar.Shared.Interfaces;
using Stellar.Shared.Interfaces.Persistence;
using Stellar.Shared.Models;
using Stellar.Shared.Utils;

namespace Stellar.Shared.Services
{
    public abstract class BaseService<E, ID, RES, REQ, PAGE_RES> : 
        IBaseService<E, ID, RES, REQ, PAGE_RES>, 
        IGetAllService<E, PAGE_RES>,
        IResponseMapper<E, RES> // Explicitly implement for single item
        where E : class
        where RES : class
        where REQ : class
        where PAGE_RES : class
    {
        public abstract ICrudPersistence<E, ID> GetCrudPersistence();

        public virtual IGetAllPersistence<E> GetGetAllPersistence()
        {
            return GetCrudPersistence() as IGetAllPersistence<E>;
        }

        public virtual RES MappingResponse(HeaderContext context, E entity)
        {
            // Default implementation using FnCommon or reflection if desired
            RES resItem = Activator.CreateInstance<RES>();
            FnCommon.CopyProperties(resItem, entity);
            return resItem;
        }

        public virtual void ValidateCreate(HeaderContext context, E entity, REQ request)
        {
        }

        public virtual void MappingCreate(HeaderContext context, E entity, REQ request)
        {
             FnCommon.CopyProperties(entity, request);
        }

        public virtual void PostCreateHandler(HeaderContext context, E entity, ID id, REQ request)
        {
        }

        public virtual void ValidateUpdateRequest(HeaderContext context, ID id, E entity, REQ request)
        {
        }

        public virtual void MappingUpdateEntity(HeaderContext context, E entity, REQ request)
        {
             FnCommon.CopyProperties(entity, request);
        }

        public virtual void PostUpdateHandler(HeaderContext context, E originalEntity, E entity, ID id, REQ request)
        {
        }

        public virtual void ValidateDelete(HeaderContext context, ID id, E entity)
        {
            // Note: IDeleteService mark ValidateDelete as protected in some versions, 
            // but here we just follow the interface if it's public or accessible.
        }

        public virtual void PostDeleteHandler(HeaderContext context, ID id, E entity)
        {
        }

        public virtual IQueryable<E> BuildFilterQuery(IQueryable<E> query, HeaderContext context, Dictionary<string, object> filter)
        {
            return query;
        }

        public virtual IQueryable<E> BuildSearchQuery(IQueryable<E> query, HeaderContext context, string? search)
        {
            return query;
        }

        public virtual string[] GetSearchFieldNames()
        {
            return new string[] { "name", "code", "ten", "ma" };
        }

        public virtual Page<PAGE_RES> MappingPageResponse(HeaderContext context, Page<E> items)
        {
            return items.Map(item =>
            {
                PAGE_RES resItem = Activator.CreateInstance<PAGE_RES>();
                FnCommon.CopyProperties(resItem, item);
                return resItem;
            });
        }
    }
}
