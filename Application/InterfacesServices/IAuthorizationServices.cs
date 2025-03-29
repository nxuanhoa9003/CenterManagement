using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.InterfacesServices
{
    public interface IAuthorizationServices
    {

        // Quản lý vai trò (Role)
        Task<bool> AddRoleAsync(string roleName);
        Task<bool> EditRoleAsync(string oldRoleName, string newRoleName);
        Task DeleteRoleAsync(string roleName);
        Task<bool> RoleExistsAsync(string roleName); // Kiểm tra vai trò có tồn tại không

        Task AssignRoleAsync(ApplicationUser user, List<string> RoleNames); // Gán vai trò cho user
        Task AssignRoleAsync(ApplicationUser user, string RoleName); // Gán vai trò cho user
        Task RemoveRoleFromUserAsync(ApplicationUser user, List<string> RoleNames); // Xóa vai trò khỏi user
        Task<IList<string>> GetUserRolesAsync(ApplicationUser user); // Lấy danh sách vai trò của user

        // Quản lý quyền (Claim)
        Task<bool> AddClaimToUserAsync(ApplicationUser user, List<Claim> claim); // Thêm claim cho user
        Task<bool> RemoveClaimFromUserAsync(ApplicationUser user, List<Claim> claim); // Xóa claim khỏi user
        Task<IList<Claim>> GetUserClaimsAsync(ApplicationUser user); // Lấy danh sách claim của user
        Task<bool> UserHasClaimAsync(ApplicationUser user, Claim claim); // Kiểm tra user đã có claim chưa

        // Quản lý vai trò với claim
        Task AddClaimToRoleAsync(string roleName, List<Claim> claims); // Thêm claim vào role
        Task RemoveClaimFromRoleAsync(string roleName, List<Claim> claims); // Xóa claim khỏi role
        Task<IList<Claim>> GetRoleClaimsAsync(string roleName); // Lấy danh sách claim của role
        Task<bool> RoleHasClaimAsync(string roleName, Claim claim);// Kiểm tra role đã có claim chưa


    }
}
