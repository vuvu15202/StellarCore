using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UserService.Application.DTOs;
using UserService.Domain.Entities;
using UserService.Domain.Repositories;
using UserService.Application.Interfaces;
using Stellar.Shared.Services;
using Stellar.Shared.Models;
using Stellar.Shared.Interfaces;
using Stellar.Shared.Interfaces.Persistence;
using Stellar.Shared.Utils;

namespace UserService.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public ICrudPersistence<User, Guid> GetCrudPersistence() => _userRepository;
        public IGetAllPersistence<User> GetGetAllPersistence() => _userRepository;

        public ProfileDTO MappingResponse(HeaderContext context, User entity)
        {
            return new ProfileDTO
            {
                UserId = entity.Id,
                FullName = entity.FullName,
                PhoneNumber = entity.PhoneNumber,
                ProfilePhotoUrl = entity.ProfilePhotoUrl,
                Email = entity.Email,
                LastLoginAt = entity.UpdatedAt, // Using UpdatedAt as LastLoginAt for auditing purposes
                UserName = entity.UserName
            };
        }

        public void MappingUpdateEntity(HeaderContext context, User entity, ProfileDTO request)
        {
            entity.FullName = request.FullName;
            entity.PhoneNumber = request.PhoneNumber;
            entity.ProfilePhotoUrl = request.ProfilePhotoUrl;
            // UserName and Email are usually not updated via ProfileDTO in this context
        }

        public async Task<bool> RegisterAsync(RegisterDTO dto)
        {
            if (await _userRepository.FindByEmailAsync(dto.Email) != null)
                return false;

            if (await _userRepository.FindByUserNameAsync(dto.UserName) != null)
                return false;

            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = dto.UserName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                FullName = dto.FullName,
                // CreatedAt/UpdatedAt handled by Persistence if inheriting from AuditingEntity
            };

            var created = await _userRepository.CreateUserAsync(user, dto.Password);
            if (!created)
                return false;

            await _userRepository.AddUserToRoleAsync(user, "Customer");

            return true;
        }

        public async Task<LoginResponseDTO> LoginAsync(LoginDTO dto, string ipAddress, string userAgent)
        {
            var response = new LoginResponseDTO();

            // Validate Client
            if (!await _userRepository.IsValidClientAsync(dto.ClientId))
            {
                response.ErrorMessage = "Invalid client ID.";
                return response;
            }

            // Get user by email or username
            var user = dto.EmailOrUserName.Contains("@")
                ? await _userRepository.FindByEmailAsync(dto.EmailOrUserName)
                : await _userRepository.FindByUserNameAsync(dto.EmailOrUserName);

            if (user == null)
            {
                response.ErrorMessage = "Invalid username or password.";
                return response;
            }

            // Check lockout info
            if (await _userRepository.IsLockedOutAsync(user))
            {
                var lockoutEnd = await _userRepository.GetLockoutEndDateAsync(user);
                if (lockoutEnd.HasValue && lockoutEnd > DateTime.UtcNow)
                {
                    var timeLeft = lockoutEnd.Value - DateTime.UtcNow;
                    response.ErrorMessage = $"Account is locked. Try again after {timeLeft.Minutes} minute(s) and {timeLeft.Seconds} second(s).";
                    response.RemainingAttempts = 0;
                    return response;
                }
                else
                {
                    await _userRepository.ResetAccessFailedCountAsync(user);
                }
            }

            if (!user.IsEmailConfirmed)
            {
                response.ErrorMessage = "Email not confirmed. Please verify your email.";
                return response;
            }

            // Validate password
            var passwordValid = await _userRepository.CheckPasswordAsync(user, dto.Password);
            if (!passwordValid)
            {
                await _userRepository.IncrementAccessFailedCountAsync(user);

                if (await _userRepository.IsLockedOutAsync(user))
                {
                    response.ErrorMessage = "Account locked due to multiple failed login attempts.";
                    response.RemainingAttempts = 0;
                    return response;
                }

                var maxAttempts = await _userRepository.GetMaxFailedAccessAttemptsAsync();
                var failedCount = await _userRepository.GetAccessFailedCountAsync(user);
                var attemptsLeft = maxAttempts - failedCount;

                response.ErrorMessage = "Invalid username or password.";
                response.RemainingAttempts = attemptsLeft > 0 ? attemptsLeft : 0;
                return response;
            }

            await _userRepository.ResetAccessFailedCountAsync(user);

            if (await _userRepository.IsTwoFactorEnabledAsync(user))
            {
                response.RequiresTwoFactor = true;
                return response;
            }

            // Update Last Login using UpdatedAt field as proxy or keep manual update
            user.UpdatedAt = DateTime.UtcNow;
            await _userRepository.UpdateUserAsync(user);

            var roles = await _userRepository.GetUserRolesAsync(user);

            response.Token = GenerateJwtToken(user, roles, dto.ClientId);
            response.RefreshToken = await _userRepository.GenerateAndStoreRefreshTokenAsync(user.Id, dto.ClientId, userAgent, ipAddress);

            return response;
        }

        public async Task<EmailConfirmationTokenResponseDTO?> SendConfirmationEmailAsync(string email)
        {
            var user = await _userRepository.FindByEmailAsync(email);
            if (user == null) return null;

            var token = await _userRepository.GenerateEmailConfirmationTokenAsync(user);
            return token != null ? new EmailConfirmationTokenResponseDTO { UserId = user.Id, Token = token } : null;
        }

        public async Task<bool> VerifyConfirmationEmailAsync(ConfirmEmailDTO dto)
        {
            var user = await _userRepository.FindByIdAsync(dto.UserId);
            if (user == null) return false;

            var result = await _userRepository.VerifyConfirmaionEmailAsync(user, dto.Token);
            if (result)
            {
                user.IsEmailConfirmed = true;
                await _userRepository.UpdateUserAsync(user);
            }
            return result;
        }

        public async Task<RefreshTokenResponseDTO> RefreshTokenAsync(RefreshTokenRequestDTO dto, string ipAddress, string userAgent)
        {
            var response = new RefreshTokenResponseDTO();

            if (!await _userRepository.IsValidClientAsync(dto.ClientId))
            {
                response.ErrorMessage = "Invalid client ID.";
                return response;
            }

            var refreshTokenEntity = await _userRepository.GetRefreshTokenAsync(dto.RefreshToken);
            if (refreshTokenEntity == null || !refreshTokenEntity.IsActive)
            {
                response.ErrorMessage = "Invalid or expired refresh token.";
                return response;
            }

            var newRefreshToken = await _userRepository.GenerateAndStoreRefreshTokenAsync(refreshTokenEntity.UserId, dto.ClientId, userAgent, ipAddress);
            var user = await _userRepository.FindByIdAsync(refreshTokenEntity.UserId);
            if (user == null)
            {
                response.ErrorMessage = "User not found.";
                return response;
            }

            var roles = await _userRepository.GetUserRolesAsync(user);
            response.Token = GenerateJwtToken(user, roles, dto.ClientId);
            response.RefreshToken = newRefreshToken;

            return response;
        }

        public async Task<bool> RevokeRefreshTokenAsync(string token, string ipAddress)
        {
            var refreshToken = await _userRepository.GetRefreshTokenAsync(token);
            if (refreshToken == null || !refreshToken.IsActive) return false;

            await _userRepository.RevokeRefreshTokenAsync(refreshToken, ipAddress);
            return true;
        }

        public async Task<ForgotPasswordResponseDTO?> ForgotPasswordAsync(string email)
        {
            var user = await _userRepository.FindByEmailAsync(email);
            if (user == null) return null;

            var token = await _userRepository.GeneratePasswordResetTokenAsync(user);
            return token != null ? new ForgotPasswordResponseDTO { UserId = user.Id, Token = token } : null;
        }

        public async Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
        {
            var user = await _userRepository.FindByIdAsync(userId);
            return user != null && await _userRepository.ChangePasswordAsync(user, currentPassword, newPassword);
        }

        public async Task<bool> ResetPasswordAsync(Guid userId, string token, string newPassword)
        {
            var user = await _userRepository.FindByIdAsync(userId);
            return user != null && await _userRepository.ResetPasswordAsync(user, token, newPassword);
        }

        public async Task<ProfileDTO?> GetProfileAsync(Guid userId)
        {
            var user = await _userRepository.FindByIdAsync(userId);
            return user != null ? MappingResponse(null!, user) : null;
        }

        public async Task<bool> UpdateProfileAsync(UpdateProfileDTO dto)
        {
            var user = await _userRepository.FindByIdAsync(dto.UserId);
            if (user == null) return false;

            user.FullName = dto.FullName;
            user.PhoneNumber = dto.PhoneNumber;
            user.ProfilePhotoUrl = dto.ProfilePhotoUrl;

            return await _userRepository.UpdateUserAsync(user);
        }

        public async Task<IEnumerable<AddressDTO>> GetAddressesAsync(Guid userId)
        {
            var addresses = await _userRepository.GetAddressesByUserIdAsync(userId);
            return addresses.Select(a => new AddressDTO
            {
                Id = a.Id,
                AddressLine1 = a.AddressLine1,
                AddressLine2 = a.AddressLine2,
                City = a.City,
                State = a.State,
                PostalCode = a.PostalCode,
                Country = a.Country,
                IsDefaultBilling = a.IsDefaultBilling,
                IsDefaultShipping = a.IsDefaultShipping
            });
        }

        public async Task<Guid> AddOrUpdateAddressAsync(AddressDTO dto)
        {
            var address = new Address
            {
                Id = dto.Id ?? Guid.NewGuid(),
                UserId = dto.userId,
                AddressLine1 = dto.AddressLine1,
                AddressLine2 = dto.AddressLine2,
                City = dto.City,
                State = dto.State,
                PostalCode = dto.PostalCode,
                Country = dto.Country,
                IsDefaultBilling = dto.IsDefaultBilling,
                IsDefaultShipping = dto.IsDefaultShipping
            };

            return await _userRepository.AddOrUpdateAddressAsync(address);
        }

        public async Task<bool> DeleteAddressAsync(Guid userId, Guid addressId)
        {
            return await _userRepository.DeleteAddressAsync(userId, addressId);
        }

        public async Task<bool> IsUserExistsAsync(Guid userId)
        {
            return await _userRepository.IsUserExistsAsync(userId);
        }

        public async Task<AddressDTO?> GetAddressByUserIdAndAddressIdAsync(Guid userId, Guid addressId)
        {
            var address = await _userRepository.GetAddressByUserIdAndAddressIdAsync(userId, addressId);
            if (address == null) return null;

            return new AddressDTO
            {
                Id = address.Id,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                City = address.City,
                State = address.State,
                PostalCode = address.PostalCode,
                Country = address.Country,
                IsDefaultBilling = address.IsDefaultBilling,
                IsDefaultShipping = address.IsDefaultShipping
            };
        }

        private string GenerateJwtToken(User user, IList<string> roles, string clientId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim(ClaimTypes.Name, user.UserName ?? ""),
                new Claim("client_id", clientId)
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var secretKey = _configuration["JwtSettings:SecretKey"];
            var issuer = _configuration["JwtSettings:Issuer"];
            var expiryMinutes = Convert.ToInt32(_configuration["JwtSettings:AccessTokenExpirationMinutes"]);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: issuer,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        // IBaseService requirements
        public RES GetById<RES>(HeaderContext context, Guid id) 
        {
            var entity = _userRepository.FindById(id);
            if (entity == null) return default!;
            return (RES)(object)MappingResponse(context, entity);
        }
        public void Delete(HeaderContext context, Guid id)
        {
             var entity = _userRepository.FindById(id);
             if (entity != null) _userRepository.Delete(entity);
        }
    }
}
