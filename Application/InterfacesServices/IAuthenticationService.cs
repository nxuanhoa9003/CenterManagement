using Domain.Entities;
using Shared.DTOs.Auth;
using Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.InterfacesServices
{
    public interface IAuthenticationService
    {
        Task<ApplicationUser> RegisterAsync(RegisterRequest model, OtpGenerator? otpmodel);
        Task<LoginResponse?> LoginAsync(LoginRequest model);
        Task<bool> CheckAccountValid(LoginRequest model);
        Task<bool> CheckUserExistById(string UserId);
        Task<bool> CheckUserExist(string username);
        Task<bool> CheckMailUserExist(string mail);
        Task<bool> IsLockedAccount(string username);
        Task<bool> IsConfirmEmailAccount(string username);
        Task<ApplicationUser?> GetUserByUserName(string username);
        Task<ApplicationUser?> GetUserById(string userId);
        Task<ApplicationUser?> GetUserByMail(string mail);
        Task FailLogin(ApplicationUser user);
        Task LockAccountAsync(ApplicationUser user);
        Task UnLockAccountAsync(ApplicationUser user);
        Task BanAccountAsync(ApplicationUser user);
        Task ResetFailLogin(ApplicationUser user);
        Task<bool> ChangePassword(ApplicationUser user, ChangePasswordRequest request);
        Task UpdateUser(ApplicationUser user);

        Task<bool> ResetPassword(ApplicationUser user, string newPassword);

        // get user by role
        Task<IEnumerable<ApplicationUser>?> GetUserByRole(string roleName);
    }
}
