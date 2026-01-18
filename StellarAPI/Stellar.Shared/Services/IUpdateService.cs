using System;
using Stellar.Shared.Interfaces;
using Stellar.Shared.Interfaces.Persistence;
using Stellar.Shared.Models;
using Stellar.Shared.Utils;

namespace Stellar.Shared.Services
{
    public interface IUpdateService<E, ID, RES, REQ> : IResponseMapper<E, RES>, ICrudPersistenceProvider<E, ID>
    {
        public RES Update(
            HeaderContext context,
            ID id,
            REQ request,
            Action<HeaderContext, ID, E, REQ> validationHandler,
            Action<HeaderContext, E, REQ> mappingUpdateAuditingEntity,
            Action<HeaderContext, E, REQ> mappingHandler,
            Action<HeaderContext, E, E, ID, REQ> postHandler,
            Func<HeaderContext, E, RES> mappingResponseHandler)
        {
            if (GetCrudPersistence() == null)
            {
                throw new ArgumentException("updatePersistence must not be null");
            }

            E entity = GetCrudPersistence().FindById(id);
            if (entity == null) throw new ArgumentException("Entity not found");

            // Deep copying for original entity might require ICloneable or serialization.
            // Using FnCommon to new instance if possible, or skip tracking original.
            E originalEntity = FnCommon.CopyNonNullProperties<E>(entity.GetType(), entity);

            if (validationHandler != null)
            {
                validationHandler(context, id, entity, request);
            }

            if (mappingUpdateAuditingEntity != null)
            {
                mappingUpdateAuditingEntity(context, entity, request);
            }

            if (mappingHandler != null)
            {
                mappingHandler(context, entity, request);
            }

            entity = GetCrudPersistence().Save(entity);

            if (postHandler != null)
            {
                postHandler(context, originalEntity, entity, id, request);
            }

            if (mappingResponseHandler == null)
            {
                throw new ArgumentException("mappingResponseHandler must not be null");
            }

            return mappingResponseHandler(context, entity);
        }

        public RES Update(HeaderContext context, ID id, REQ request)
        {
            if (GetCrudPersistence() == null)
            {
                throw new ArgumentException("updatePersistence must not be null");
            }

            return Update(
                context,
                id,
                request,
                ValidateUpdateRequest,
                MappingUpdateAuditingEntity,
                MappingUpdateEntity,
                PostUpdateHandler,
                MappingResponse);
        }

        public void ValidateUpdateRequest(HeaderContext context, ID id, E entity, REQ request) { }

        public void MappingUpdateEntity(HeaderContext context, E entity, REQ request)
        {
             FnCommon.CopyProperties(entity, request);
        }

        public void PostUpdateHandler(HeaderContext context, E originalEntity, E entity, ID id, REQ request) { }

        public void MappingUpdateAuditingEntity(HeaderContext context, E entity, REQ request)
        {
             if (context != null && entity is AuditingEntity auditingEntity)
             {
                 auditingEntity.UpdatedBy = context.Username;
             }
        }
    }
}
