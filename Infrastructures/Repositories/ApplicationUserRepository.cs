using Domain.Entities;
using Domain.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repositories
{
    public class ApplicationUserRepository : GenericRepository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;


        public ApplicationUserRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context, IPasswordHasher<ApplicationUser> passwordHasher) : base(context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _passwordHasher = passwordHasher;
        }


        // Đăng ký tài khoản
        public async Task<ApplicationUser> RegisterAsync(ApplicationUser user, string password)
        {
            // Băm mật khẩu trước khi lưu
            user.PasswordHash = _passwordHasher.HashPassword(user, password);
            await InsertAsync(user);
            return user;
        }

        // Cập nhật mật khẩu tài khoản
        public async Task<bool> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword)
        {
            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }
            return result.Succeeded;
        }

        // Kiểm tra tài khoản hợp lệ
        public async Task<bool> CheckAccountValidAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            return (user != null && await _userManager.CheckPasswordAsync(user, password));
        }

        // Tăng số lần đăng nhập thất bại
        public async Task FailLoginAsync(ApplicationUser user)
        {
            user.AccessFailedCount++;
            await UpdateAsync(user);
        }
        // Lấy danh sách tất cả user
        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            return await GetAllAsync();
        }

        // Lấy user theo ID
        public async Task<ApplicationUser?> GetByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        // Lấy user theo username
        public async Task<ApplicationUser?> GetByUserNameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        // Lấy ID theo username
        public async Task<string> GetUserIdAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return user?.Id ?? string.Empty;
        }

        // Lấy danh sách user theo role
        public async Task<IEnumerable<ApplicationUser>> GetUsersByRoleAsync(string role)
        {
            var users = await _userManager.GetUsersInRoleAsync(role);
            return users;
        }

        // Kiểm tra tài khoản theo ID
        public async Task<bool> IsHasUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId) != null;
        }

        // Kiểm tra tài khoản bị khóa
        public async Task<bool> IsLockedAccountAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return true;
            return user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.UtcNow;
        }

        // Kiểm tra tài khoản xác nhận
        public async Task<bool> IsConfirmEmailAccountAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return user?.EmailConfirmed ?? false;
        }

        // Kiểm tra username có tồn tại
        public async Task<bool> IsUserNameExistsAsync(string username)
        {
            return await _userManager.FindByNameAsync(username) != null;
        }

        // Kiểm tra mail có tồn tại
        public async Task<bool> IsMailExistsAsync(string mail)
        {
            return await _userManager.FindByEmailAsync(mail) != null;
        }

        // Cấm tài khoản
        public async Task BanAccountAsync(ApplicationUser user)
        {
            user.LockoutEnabled = true;
            user.LockoutEnd = DateTime.MaxValue; // Khóa tài khoản vô thời hạn
            await UpdateAsync(user);
        }

        // Khóa tài khoản sau nhiều lần đăng nhập sai
        public async Task LockAccountAsync(ApplicationUser user)
        {
            user.LockoutEnabled = true;
            user.LockoutEnd = DateTime.UtcNow.AddMinutes(15); // Khóa tài khoản 15 phút
            await UpdateAsync(user);
        }

        // Reset số lần đăng nhập thất bại
        public async Task ResetFailLoginAsync(ApplicationUser user)
        {
            user.AccessFailedCount = 0;
            user.LockoutEnd = null;
            await UpdateAsync(user);
        }

        public async Task UpdateUserAsync(ApplicationUser user)
        {
            await _userManager.UpdateAsync(user);
        }

        public async Task<ApplicationUser?> GetByMailAsync(string mail)
        {
            return await _userManager.FindByEmailAsync(mail);
        }

        public async Task<bool> ResetPasswordAsync(ApplicationUser user, string newPassword)
        {
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetResult = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
            return resetResult.Succeeded;
        }

        public async Task<IEnumerable<ApplicationUser>?> GetUserByRoleAsync(string role)
        {
            return await _userManager.GetUsersInRoleAsync(role);
        }
    }
}
