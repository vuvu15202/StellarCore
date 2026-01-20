using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Stellar.Shared.Repositories;
using UserService.Domain.Models.Entities;
using UserService.Domain.Services.Persistence;
using UserService.Infrastructure.Database;
using UserService.Infrastructure.Identity;

namespace UserService.Infrastructure.Services.Repositories
{
    public class RoleRepository : CrudRepository<Role, Guid>, RolePersistence
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RoleRepository(
            RoleManager<ApplicationRole> roleManager,
            UserDbContext dbContext) : base(dbContext)
        {
            _roleManager = roleManager;
        }

        private Role MapToDomain(ApplicationRole appRole)
        {
            if (appRole == null) return null!;
            return new Role
            {
                Id = appRole.Id,
                Name = appRole.Name,
                Description = appRole.Description
            };
        }

        private ApplicationRole MapToApplicationRole(Role role)
        {
            return new ApplicationRole
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description
            };
        }

        public new IQueryable<Role> Query()
        {
            return _roleManager.Roles.Select(r => new Role
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description
            });
        }

        public new Role? FindById(Guid id)
        {
            var appRole = _roleManager.FindByIdAsync(id.ToString()).GetAwaiter().GetResult();
            return appRole != null ? MapToDomain(appRole) : null;
        }

        public Role Save(Role entity)
        {
            var appRole = _roleManager.FindByIdAsync(entity.Id.ToString()).GetAwaiter().GetResult();
            if (appRole == null)
            {
                var newAppRole = MapToApplicationRole(entity);
                _roleManager.CreateAsync(newAppRole).GetAwaiter().GetResult();
                return MapToDomain(newAppRole);
            }

            appRole.Name = entity.Name;
            appRole.Description = entity.Description;

            _roleManager.UpdateAsync(appRole).GetAwaiter().GetResult();
            return MapToDomain(appRole);
        }

        public void Delete(Role entity)
        {
            var appRole = _roleManager.FindByIdAsync(entity.Id.ToString()).GetAwaiter().GetResult();
            if (appRole != null)
            {
                _roleManager.DeleteAsync(appRole).GetAwaiter().GetResult();
            }
        }

        public new bool ExistsById(Guid id)
        {
            return _roleManager.FindByIdAsync(id.ToString()).GetAwaiter().GetResult() != null;
        }
    }
}
