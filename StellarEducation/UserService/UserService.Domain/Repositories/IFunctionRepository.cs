using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain.Entities;
using Stellar.Shared.Interfaces;
using Stellar.Shared.Interfaces.Persistence;

namespace UserService.Domain.Repositories
{
    public interface IFunctionRepository : ICrudPersistence<Function, Guid>, IGetAllPersistence<Function>
    {
        Task<List<Function>> GetChildrenByParentIds(List<Guid> parentIds);
        Task<List<Function>> GetFunctionsByUserId(Guid userId);
        Task<List<Function>> FindByParentId(Guid parentId);
        Task<List<Function>> FindAllByIdIn(List<Guid> ids);
    }
}
