using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stellar.Shared.Services;
using UserService.Application.DTOs;
using UserService.Domain.Models.Entities;

namespace UserService.Application.Usecases.Interfaces
{
    public interface IUserService : IBaseService<User, Guid, ProfileDTO, ProfileDTO, ProfileDTO>
    {
        Task<bool> RegisterAsync(RegisterDTO dto);
        Task<LoginResponseDTO> LoginAsync(LoginDTO dto, string ipAddress, string userAgent);
        Task<EmailConfirmationTokenResponseDTO?> SendConfirmationEmailAsync(string email);
        Task<bool> VerifyConfirmationEmailAsync(ConfirmEmailDTO dto);
        Task<RefreshTokenResponseDTO> RefreshTokenAsync(RefreshTokenRequestDTO dto, string ipAddress, string userAgent);
        Task<bool> RevokeRefreshTokenAsync(string token, string ipAddress);
        Task<ForgotPasswordResponseDTO?> ForgotPasswordAsync(string email);
        Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);
        Task<bool> ResetPasswordAsync(Guid userId, string token, string newPassword);
        Task<ProfileDTO?> GetProfileAsync(Guid userId);
        Task<bool> UpdateProfileAsync(UpdateProfileDTO dto);
        Task<IEnumerable<AddressDTO>> GetAddressesAsync(Guid userId);
        Task<Guid> AddOrUpdateAddressAsync(AddressDTO dto);
        Task<bool> DeleteAddressAsync(Guid userId, Guid addressId);
        Task<bool> IsUserExistsAsync(Guid userId);
        Task<AddressDTO?> GetAddressByUserIdAndAddressIdAsync(Guid userId, Guid addressId);
    }
}
