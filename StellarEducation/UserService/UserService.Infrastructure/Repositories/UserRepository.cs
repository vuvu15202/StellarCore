using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;
using UserService.Domain.Repositories;
using UserService.Infrastructure.Identity;
using UserService.Infrastructure.Persistence;
using Stellar.Shared.Repositories;
using Stellar.Shared.Interfaces;

namespace UserService.Infrastructure.Repositories
{
    public class UserRepository : CrudRepository<User, Guid>, IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(
            UserManager<ApplicationUser> userManager,
            UserDbContext dbContext) : base(dbContext)
        {
            _userManager = userManager;
        }

        private UserDbContext UserDbContext => (UserDbContext)Context;

        private User MapToDomain(ApplicationUser appUser)
        {
            if (appUser == null) return null!;
            return new User
            {
                Id = appUser.Id,
                UserName = appUser.UserName,
                Email = appUser.Email,
                FullName = appUser.FullName,
                PhoneNumber = appUser.PhoneNumber,
                ProfilePhotoUrl = appUser.ProfilePhotoUrl,
                IsActive = appUser.IsActive,
                IsEmailConfirmed = appUser.EmailConfirmed,
                CreatedAt = appUser.CreatedAt,
                UpdatedAt = appUser.LastLoginAt ?? appUser.CreatedAt, // AuditingEntity fields
                CreatedBy = "System", // Or fetch from context if possible
                UpdatedBy = "System"
            };
        }

        private ApplicationUser MapToApplicationUser(User user)
        {
            return new ApplicationUser
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                ProfilePhotoUrl = user.ProfilePhotoUrl,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.UpdatedAt,
                EmailConfirmed = true // Default or handle via logic
            };
        }

        public async Task<User?> FindByEmailAsync(string email)
        {
            var appUser = await _userManager.FindByEmailAsync(email);
            return appUser != null ? MapToDomain(appUser) : null;
        }

        public async Task<User?> FindByUserNameAsync(string userName)
        {
            var appUser = await _userManager.FindByNameAsync(userName);
            return appUser != null ? MapToDomain(appUser) : null;
        }

        public async Task<User?> FindByIdAsync(Guid id)
        {
            var appUser = await _userManager.FindByIdAsync(id.ToString());
            return appUser != null ? MapToDomain(appUser) : null;
        }

        public async Task<bool> CreateUserAsync(User user, string password)
        {
            var appUser = MapToApplicationUser(user);
            var result = await _userManager.CreateAsync(appUser, password);
            return result.Succeeded;
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
            return appUser != null && await _userManager.CheckPasswordAsync(appUser, password);
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
            if (appUser == null) return false;

            appUser.UserName = user.UserName;
            appUser.Email = user.Email;
            appUser.FullName = user.FullName;
            appUser.PhoneNumber = user.PhoneNumber;
            appUser.ProfilePhotoUrl = user.ProfilePhotoUrl;

            var result = await _userManager.UpdateAsync(appUser);
            return result.Succeeded;
        }

        public async Task<IList<string>> GetUserRolesAsync(User user)
        {
            var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
            return appUser != null ? await _userManager.GetRolesAsync(appUser) : new List<string>();
        }

        public async Task<bool> AddUserToRoleAsync(User user, string role)
        {
            var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
            return appUser != null && (await _userManager.AddToRoleAsync(appUser, role)).Succeeded;
        }

        public async Task<string?> GenerateEmailConfirmationTokenAsync(User user)
        {
            var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
            return appUser != null ? await _userManager.GenerateEmailConfirmationTokenAsync(appUser) : null;
        }

        public async Task<bool> VerifyConfirmaionEmailAsync(User user, string token)
        {
            var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
            return appUser != null && (await _userManager.ConfirmEmailAsync(appUser, token)).Succeeded;
        }

        public async Task<string?> GeneratePasswordResetTokenAsync(User user)
        {
            var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
            return appUser != null ? await _userManager.GeneratePasswordResetTokenAsync(appUser) : null;
        }

        public async Task<bool> ResetPasswordAsync(User user, string token, string newPassword)
        {
            var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
            return appUser != null && (await _userManager.ResetPasswordAsync(appUser, token, newPassword)).Succeeded;
        }

        public async Task<bool> ChangePasswordAsync(User user, string currentPassword, string newPassword)
        {
            var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
            return appUser != null && (await _userManager.ChangePasswordAsync(appUser, currentPassword, newPassword)).Succeeded;
        }

        public async Task UpdateLastLoginAsync(User user, DateTime loginTime)
        {
            var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
            if (appUser != null)
            {
                appUser.LastLoginAt = loginTime;
                await _userManager.UpdateAsync(appUser);
            }
        }

        public async Task<string> GenerateAndStoreRefreshTokenAsync(Guid userId, string clientId, string userAgent, string ipAddress)
        {
            // Placeholder for refresh token logic, simplified
            return await Task.FromResult(Guid.NewGuid().ToString());
        }

        public async Task<RefreshToken?> GetRefreshTokenAsync(string token)
        {
            return await Task.FromResult<RefreshToken?>(null);
        }

        public async Task RevokeRefreshTokenAsync(RefreshToken refreshToken, string ipAddress)
        {
            await Task.CompletedTask;
        }

        public async Task<List<Address>> GetAddressesByUserIdAsync(Guid userId)
        {
            return await Context.Set<Address>().Where(a => a.UserId == userId).ToListAsync();
        }

        public async Task<Guid> AddOrUpdateAddressAsync(Address address)
        {
            var existing = await Context.Set<Address>().FindAsync(address.Id);
            if (existing == null)
            {
                Context.Set<Address>().Add(address);
                await Context.SaveChangesAsync();
                return address.Id;
            }
            Context.Entry(existing).CurrentValues.SetValues(address);
            await Context.SaveChangesAsync();
            return existing.Id;
        }

        public async Task<bool> DeleteAddressAsync(Guid userId, Guid addressId)
        {
            var address = await Context.Set<Address>().FirstOrDefaultAsync(a => a.Id == addressId && a.UserId == userId);
            if (address == null) return false;
            Context.Set<Address>().Remove(address);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsLockedOutAsync(User user)
        {
            var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
            return appUser != null && await _userManager.IsLockedOutAsync(appUser);
        }

        public async Task<bool> IsTwoFactorEnabledAsync(User user)
        {
            var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
            return appUser != null && await _userManager.GetTwoFactorEnabledAsync(appUser);
        }

        public async Task IncrementAccessFailedCountAsync(User user)
        {
            var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
            if (appUser != null) await _userManager.AccessFailedAsync(appUser);
        }

        public async Task ResetAccessFailedCountAsync(User user)
        {
            var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
            if (appUser != null) await _userManager.ResetAccessFailedCountAsync(appUser);
        }

        public async Task<DateTime?> GetLockoutEndDateAsync(User user)
        {
            var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
            return appUser?.LockoutEnd?.UtcDateTime;
        }

        public Task<int> GetMaxFailedAccessAttemptsAsync()
        {
            return Task.FromResult(_userManager.Options.Lockout.MaxFailedAccessAttempts);
        }

        public async Task<int> GetAccessFailedCountAsync(User user)
        {
            var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
            return appUser?.AccessFailedCount ?? 0;
        }

        public async Task<bool> IsValidClientAsync(string clientId)
        {
            return await Context.Set<Client>().AnyAsync(c => c.ClientId == clientId);
        }

        public async Task<bool> IsUserExistsAsync(Guid userId)
        {
            return await Context.Set<ApplicationUser>().AnyAsync(u => u.Id == userId);
        }

        public async Task<bool> IsRoleExistsAsync(Guid roleId)
        {
            return await Context.Set<ApplicationRole>().AnyAsync(r => r.Id == roleId);
        }

        public async Task<List<Guid>> GetRoleIdsByUserIdAsync(Guid userId)
        {
            return await UserDbContext.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.RoleId)
                .ToListAsync();
        }

        public async Task<Address?> GetAddressByUserIdAndAddressIdAsync(Guid userId, Guid addressId)
        {
            return await Context.Set<Address>().AsNoTracking().FirstOrDefaultAsync(a => a.UserId == userId && a.Id == addressId);
        }
    }
}
