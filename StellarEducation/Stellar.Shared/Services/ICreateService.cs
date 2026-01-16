using System;
using Stellar.Shared.Interfaces;
using Stellar.Shared.Interfaces.Persistence;
using Stellar.Shared.Models;
using Stellar.Shared.Utils;

namespace Stellar.Shared.Services
{
    public interface ICreateService<E, ID, RES, REQ> :
    ICrudPersistenceProvider<E, ID>,
    IResponseMapper<E, RES>
    {
        public RES Create(HeaderContext context, REQ request)
        {
             return Create(context, request, ValidateCreate, MappingCreate, PostCreateHandler, MappingResponse);
        }

        public RES Create(
            HeaderContext context,
            REQ request,
            Action<HeaderContext, E, REQ> validationHandler,
            Action<HeaderContext, E, REQ> mappingHandler,
            Action<HeaderContext, E, ID, REQ> postHandler,
            Func<HeaderContext, E, RES> mappingResponseHandler)
        {
             if (GetCrudPersistence() == null) throw new ArgumentException("Persistence cannot be null");

             // Create new instance of E
             // Using generic new() constraint or Activator
             E entity = Activator.CreateInstance<E>();

             if (validationHandler != null) validationHandler(context, entity, request);

             if (mappingHandler != null) mappingHandler(context, entity, request);
             
             // Handle Auditing if applicable
             if (entity is AuditingEntity auditingEntity && context != null)
             {
                 auditingEntity.CreatedBy = context.Username;
                 auditingEntity.UpdatedBy = context.Username;
                 auditingEntity.CreatedAt = DateTime.UtcNow;
                 auditingEntity.UpdatedAt = DateTime.UtcNow;
                 auditingEntity.Id = Guid.NewGuid(); // Or handle ID generation strategy
             }

             entity = GetCrudPersistence().Save(entity);

             // ID is now populated? If Guid, we set it. If Int DB generated, it's set.
             // We need ID for postHandler.
             // Assuming Persistence returns saved entity with ID.
             
             // How to get ID generic? E doesn't constrain ID access. 
             // We cast to AuditingEntity or assume implementation knows.
             // Or we rely on E having ID property.
             // For now, let's assume Save returns updated entity.

             // We need to cast entity ID to ID type or have a way to extract it.
             // This is tricky with generics.
             // However, for PostHandler, we pass ID.
             
             ID id = default;
             if (entity is AuditingEntity ae && typeof(ID) == typeof(Guid))
             {
                 id = (ID)(object)ae.Id;
             }
             // For simplicity in this port, we bypass ID extraction if not easy, or rely on Entity being same instance.
             
             if (postHandler != null) postHandler(context, entity, id, request);

             return mappingResponseHandler(context, entity);
        }

        void ValidateCreate(HeaderContext context, E entity, REQ request) { }

        void MappingCreate(HeaderContext context, E entity, REQ request)
            => FnCommon.CopyProperties(entity, request);

        void PostCreateHandler(HeaderContext context, E entity, ID id, REQ request) { }
    }
}
