using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stellar.Shared.Services;
using UserService.Domain.Entities;
using UserService.Application.DTOs.Requests;
using UserService.Application.DTOs.Responses;

namespace UserService.Application.Interfaces
{
    public interface IFunctionService : IBaseService<Function, Guid, FunctionResponse, FunctionRequest, FunctionResponse>
    {
        Task<List<Function>> GetFunctionIds(Guid userId);
    }
}
