using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stellar.Shared.Interfaces.Persistence;
using Stellar.Shared.Models;
using Stellar.Shared.Services;
using UserService.Application.DTOs.Requests;
using UserService.Application.DTOs.Responses;
using UserService.Application.Usecases.Interfaces;
using UserService.Domain.Models.Entities;
using UserService.Domain.Services.Persistence;

namespace UserService.Application.Usecases
{
    public class PlanService : BaseService<Plan, Guid, PlanResponse, PlanRequest, PlanResponse>, IPlanService
    {
        private readonly PlanPersistence _persistence;
        private readonly RelationPlanFunctionPersistence _planFunctionRepository;
        private readonly FunctionPersistence _functionRepository;

        public PlanService(
            PlanPersistence persistence,
            RelationPlanFunctionPersistence planFunctionRepository,
            FunctionPersistence functionRepository)
        {
            _persistence = persistence;
            _planFunctionRepository = planFunctionRepository;
            _functionRepository = functionRepository;
        }

        public override ICrudPersistence<Plan, Guid> GetCrudPersistence() => _persistence;

        public async Task AddPermission(PlanPermissionRequest request)
        {
            // Check if plan exists
            var plan = _persistence.FindById(request.PlanId);
            if (plan == null)
            {
                throw new ArgumentException($"Plan with ID {request.PlanId} does not exist.");
            }

            // Check if all functionIds exist
            var distinctFunctionIds = request.FunctionIds.Distinct().ToList();
            var existingFunctions = await _functionRepository.FindAllByIdIn(distinctFunctionIds);
            if (existingFunctions.Count != distinctFunctionIds.Count)
            {
                var missingIds = distinctFunctionIds.Except(existingFunctions.Select(f => f.Id)).ToList();
                throw new ArgumentException($"The following Function IDs do not exist: {string.Join(", ", missingIds)}");
            }

            // Delete existing permissions for this plan
            var existingRelations = await _planFunctionRepository.FindByPlanId(request.PlanId);
            foreach (var rel in existingRelations)
            {
                _planFunctionRepository.Delete(rel);
            }

            // Add new permissions
            foreach (var functionId in distinctFunctionIds)
            {
                var relation = new RelationPlanFunction
                {
                    Id = Guid.NewGuid(),
                    PlanId = request.PlanId,
                    FunctionId = functionId,
                };
                _planFunctionRepository.Save(relation);
            }
        }
    }
}
