using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Application.DTOs.Requests;
using UserService.Application.DTOs.Responses;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;
using UserService.Domain.Repositories;
using Stellar.Shared.Models;
using Stellar.Shared.Interfaces;
using Stellar.Shared.Interfaces.Persistence;
using Stellar.Shared.Utils;

namespace UserService.Application.Services
{
    public class RelationRoleFunctionService : IRelationRoleFunctionService
    {
        private readonly IRelationRoleFunctionRepository _persistence;

        public RelationRoleFunctionService(IRelationRoleFunctionRepository persistence)
        {
            _persistence = persistence;
        }

        public ICrudPersistence<RelationRoleFunction, Guid> GetCrudPersistence() => _persistence;
        public IGetAllPersistence<RelationRoleFunction> GetGetAllPersistence() => _persistence;

        public void MappingCreate(HeaderContext context, RelationRoleFunction entity, RelationRoleFunctionRequest request)
        {
            FnCommon.CopyProperties(entity, request);
            entity.Id = Guid.NewGuid();
        }

        public void MappingUpdateEntity(HeaderContext context, RelationRoleFunction entity, RelationRoleFunctionRequest request)
        {
            FnCommon.CopyProperties(entity, request);
        }

        public RelationRoleFunctionResponse MappingResponse(HeaderContext context, RelationRoleFunction entity)
        {
            return new RelationRoleFunctionResponse
            {
                Id = entity.Id,
                RoleId = entity.RoleId,
                FunctionId = entity.FunctionId,
                CreatedBy = entity.CreatedBy,
                CreatedAt = entity.CreatedAt
            };
        }

        public RES GetById<RES>(HeaderContext context, Guid id)
        {
            var entity = _persistence.FindById(id);
            if (entity == null) return default!;
            return (RES)(object)MappingResponse(context, entity);
        }

        public void Delete(HeaderContext context, Guid id)
        {
            var entity = _persistence.FindById(id);
            if (entity != null) _persistence.Delete(entity);
        }
    }
}
