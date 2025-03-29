using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IApplicationUserRepository : IGenericRepository<ApplicationUser>
    {
        Task<ApplicationUser> RegisterAsync(ApplicationUser user, string password);
        Task<bool> ChangePasswordAsync(ApplicationUser user, string currentPassword, string password);
        Task FailLoginAsync(ApplicationUser user);
        Task LockAccountAsync(ApplicationUser user);
        Task ResetFailLoginAsync(ApplicationUser user);
        Task BanAccountAsync(ApplicationUser user);
        Task<bool> CheckAccountValidAsync(string username, string password);
        Task<bool> IsLockedAccountAsync(string username);
        Task<bool> IsConfirmEmailAccountAsync(string username);
        Task<bool> IsUserNameExistsAsync(string username);
        Task<bool> IsMailExistsAsync(string mail);
        Task<bool> IsHasUserByIdAsync(string userId);
        Task<string> GetUserIdAsync(string username);
        Task<ApplicationUser?> GetByIdAsync(string userId);
        Task<ApplicationUser?> GetByUserNameAsync(string username);
        Task<ApplicationUser?> GetByMailAsync(string mail);
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        Task<IEnumerable<ApplicationUser>> GetUsersByRoleAsync(string role);

        Task UpdateUserAsync(ApplicationUser user);
        Task<bool> ResetPasswordAsync(ApplicationUser user, string newPassword);

        // get list user by role 
        Task<IEnumerable<ApplicationUser>?> GetUserByRoleAsync(string role);
    }
}
