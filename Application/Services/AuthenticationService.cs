using Domain.Entities;
using Domain.Interface;
using Application.InterfacesServices;
using Infrastructures.Repositories;
using Microsoft.AspNetCore.Identity;
using Shared.DTOs.Auth;
using Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IApplicationUserRepository _userRepository;

        public AuthenticationService(IApplicationUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // REGISTER
        public async Task<ApplicationUser> RegisterAsync(RegisterRequest model, OtpGenerator? otpmodel)
        {

            var username = model.Username?.Trim() ?? string.Empty; // Xóa khoảng trắng đầu/cuối, tránh null
            var email = model.Email?.Trim() ?? string.Empty;

            var user = new ApplicationUser
            {
                UserName = model.Username,
                NormalizedUserName = username.ToUpperInvariant(), // Dùng `ToUpperInvariant()` để nhất quán ,
                Email = model.Email,
                NormalizedEmail = email.ToUpperInvariant(),
                FullName = model.FullName?.Trim(),
                Otp = otpmodel?.OtpCode ?? string.Empty,  // Nếu null, gán chuỗi rỗng
                OtpExpiryDate = otpmodel?.ExpiryTime ?? DateTime.MinValue, // Nếu null, gán giá trị mặc định
                LastOtpSentTime = DateTime.UtcNow,
            };
            return await _userRepository.RegisterAsync(user, model.Password);
        }

        // LOGIN
        public async Task<LoginResponse?> LoginAsync(LoginRequest model)
        {
            var user = await _userRepository.GetByUserNameAsync(model.Username);
            if (user == null)
            {
                return null;
            }

            return new LoginResponse
            {
                UserId = user.Id,
                Username = user.UserName,
                Email = user.Email,
                FullName = user.FullName
            };
        }

        public async Task<bool> CheckAccountValid(LoginRequest model)
        {
            return await _userRepository.CheckAccountValidAsync(model.Username, model.Password);
        }

        public async Task<bool> CheckUserExistById(string UserId)
        {
            return await _userRepository.IsHasUserByIdAsync(UserId);
        }


        public async Task<bool> CheckUserExist(string username)
        {
            return await _userRepository.IsUserNameExistsAsync(username);
        }

        public async Task<bool> CheckMailUserExist(string mail)
        {
            return await _userRepository.IsMailExistsAsync(mail);
        }

        public async Task<bool> IsLockedAccount(string username)
        {
            return await _userRepository.IsLockedAccountAsync(username);
        }

        public async Task<ApplicationUser?> GetUserByUserName(string username)
        {
            return await _userRepository.GetByUserNameAsync(username);
        }

        public async Task<ApplicationUser?> GetUserByMail(string mail)
        {
            return await _userRepository.GetByMailAsync(mail);
        }

        public async Task<ApplicationUser?> GetUserById(string userId)
        {
            return await _userRepository.GetByIdAsync(userId);
        }

        public async Task FailLogin(ApplicationUser user)
        {
            await _userRepository.FailLoginAsync(user);
            var failLogin = user?.AccessFailedCount ?? 0;
            if (failLogin >= 2)
                await _userRepository.LockAccountAsync(user);
            if (failLogin < 10) return;
            await _userRepository.BanAccountAsync(user);
        }

        public async Task ResetFailLogin(ApplicationUser user)
        {
            await _userRepository.ResetFailLoginAsync(user);
        }


        public async Task<bool> ChangePassword(ApplicationUser user, ChangePasswordRequest request)
        {
            return await _userRepository.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        }

        public async Task<bool> IsConfirmEmailAccount(string username)
        {
            return await _userRepository.IsConfirmEmailAccountAsync(username);
        }

        public async Task UpdateUser(ApplicationUser user)
        {
            await _userRepository.UpdateUserAsync(user);
        }

        public async Task<bool> ResetPassword(ApplicationUser user, string newPassword)
        {
            return await _userRepository.ResetPasswordAsync(user, newPassword);
        }

        public async Task LockAccountAsync(ApplicationUser user)
        {
            await _userRepository.LockAccountAsync(user);
        }
        public async Task BanAccountAsync(ApplicationUser user)
        {
            await _userRepository.BanAccountAsync(user);
        }

        public async Task UnLockAccountAsync(ApplicationUser user)
        {
            await _userRepository.ResetFailLoginAsync(user);
        }

        public async Task<IEnumerable<ApplicationUser>?> GetUserByRole(string roleName)
        {
            return await _userRepository.GetUserByRoleAsync(roleName);
        }
    }
}
