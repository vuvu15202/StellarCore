using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserService.Application.DTOs.Requests;
using UserService.Application.DTOs.Responses;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;
using UserService.Domain.Repositories;
using UserService.Domain.Enums;

namespace UserService.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRelationRoleFunctionRepository _roleFunctionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFunctionRepository _functionRepository;
        private readonly IFunctionService _functionService;
        private readonly IUserPlanSubscriptionRepository _subscriptionRepository;
        private readonly IRelationPlanFunctionRepository _planFunctionRepository;
        private readonly IPlanRepository _planRepository;

        public RoleService(
            IRelationRoleFunctionRepository roleFunctionRepository,
            IUserRepository userRepository,
            IFunctionRepository functionRepository,
            IFunctionService functionService,
            IUserPlanSubscriptionRepository subscriptionRepository,
            IRelationPlanFunctionRepository planFunctionRepository,
            IPlanRepository planRepository)
        {
            _roleFunctionRepository = roleFunctionRepository;
            _userRepository = userRepository;
            _functionRepository = functionRepository;
            _functionService = functionService;
            _subscriptionRepository = subscriptionRepository;
            _planFunctionRepository = planFunctionRepository;
            _planRepository = planRepository;
        }

        public async Task AddPermission(PermissionRequest request)
        {
            // Check if role exists
            if (!await _userRepository.IsRoleExistsAsync(request.RoleId))
            {
                throw new ArgumentException($"Role with ID {request.RoleId} does not exist.");
            }

            // Check if all functionIds exist
            var distinctFunctionIds = request.FunctionIds.Distinct().ToList();
            var existingFunctions = await _functionRepository.FindAllByIdIn(distinctFunctionIds);
            if (existingFunctions.Count != distinctFunctionIds.Count)
            {
                var missingIds = distinctFunctionIds.Except(existingFunctions.Select(f => f.Id)).ToList();
                throw new ArgumentException($"The following Function IDs do not exist: {string.Join(", ", missingIds)}");
            }

            // Delete existing permissions for this role
            await _roleFunctionRepository.DeleteByRoleId(request.RoleId);

            // Add new permissions
            foreach (var functionId in distinctFunctionIds)
            {
                var relation = new RelationRoleFunction
                {
                    Id = Guid.NewGuid(),
                    RoleId = request.RoleId,
                    FunctionId = functionId,
                };
                _roleFunctionRepository.Save(relation);
            }
        }

        public async Task<MenuResponse> GetPermission(Guid userId)
        {
            var response = new MenuResponse();

            // Get role IDs for the user
            var roleIds = await _userRepository.GetRoleIdsByUserIdAsync(userId);
            if (roleIds == null || !roleIds.Any())
            {
                return response;
            }

            // Get function IDs for these roles
            var functionPermissionIds = new HashSet<Guid>();
            foreach (var roleId in roleIds)
            {
                var relations = await _roleFunctionRepository.FindByRoleId(roleId);
                foreach (var rel in relations)
                {
                    functionPermissionIds.Add(rel.FunctionId);
                }
            }

            // --- PLAN BASED PERMISSIONS ---
            Guid? activePlanId = null;
            var activeSub = await _subscriptionRepository.GetActiveSubscriptionByUserId(userId);
            if (activeSub != null)
            {
                activePlanId = activeSub.PlanId;
            }
            else
            {
                // Fallback to Trial Plan
                // For simplicity, find the first free plan. 
                // Better logic would be matching roles to PlanType.
                var trialPlans = await _planRepository.Query().ToListAsync();
                var defaultTrial = trialPlans.FirstOrDefault(p => p.Price == 0 && p.IsActive);
                if (defaultTrial != null)
                {
                    activePlanId = defaultTrial.Id;
                }
            }

            if (activePlanId.HasValue)
            {
                var planFunctionIds = await _planFunctionRepository.FindByPlanId(activePlanId.Value);
                foreach (var pf in planFunctionIds)
                {
                    functionPermissionIds.Add(pf.FunctionId);
                }
            }
            // ------------------------------

            if (!functionPermissionIds.Any())
            {
                return response;
            }

            // Get function entities
            var functions = await _functionRepository.FindAllByIdIn(functionPermissionIds.ToList());

            // User Java's buildMenuFromChucNangs logic mapping
            // Filter Permissions (Action)
            response.Permissions = functions
                .Where(f => f.Type == FunctionType.Action)
                .Select(f => f.Code)
                .Distinct()
                .ToList();

            // Filter Menus (Navigation)
            var navigationFunctions = functions
                .Where(f => f.Type == FunctionType.Navigation)
                .OrderBy(f => f.SortOrder) // Assuming SortOrder exists based on Function entity
                .ToList();
            
            response.Menus = BuildTreeFromFunctions(navigationFunctions);

            return response;
        }

        private List<FunctionResponse> BuildTreeFromFunctions(List<Function> functions)
        {
            var nodeMap = new Dictionary<Guid, FunctionResponse>();
            // Map to Response DTOs
            foreach (var func in functions)
            {
                // We need to map Function to FunctionResponse here. 
                // Since _functionService.MappingResponse might require dependencies we can't easily access or mock, 
                // and we need a clean tree, we'll map manually or reuse if possible.
                // Reusing _functionService.MappingResponse(null, func) as per existing code hint, 
                // but passing null context might be risky if it uses it.
                // Let's implement a simple mapping here to sure.

                 var dto = new FunctionResponse
                 {
                     Id = func.Id,
                     Code = func.Code,
                     Name = func.Name,
                     Path = func.Path,
                     SortOrder = func.SortOrder,
                     SortPath = func.SortPath,
                     Icon = func.Icon,
                     Description = func.Description,
                     HierarchyPath = func.HierarchyPath,
                     Category = func.Category,
                     ParentId = func.ParentId,
                     Type = func.Type,
                     CreatedBy = func.CreatedBy,
                     LastModifiedBy = func.UpdatedBy,
                     CreatedAt = func.CreatedAt,
                     LastModifiedAt = func.UpdatedAt,
                     Children = new List<FunctionResponse>()
                 };
                 nodeMap[func.Id] = dto;
            }

            var roots = new List<FunctionResponse>();

            foreach (var node in nodeMap.Values)
            {
                 if (node.ParentId.HasValue && nodeMap.ContainsKey(node.ParentId.Value))
                 {
                     nodeMap[node.ParentId.Value].Children.Add(node);
                 }
                 else
                 {
                     roots.Add(node);
                 }
            }

            return roots;
        }
    }
}
