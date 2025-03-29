using Domain.Entities;
using Domain.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repositories
{
    public class AuthorizationRepository : GenericRepository<ApplicationUser>, IAuthorizationRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;


        public AuthorizationRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context) : base(context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }


        // Quản lý vai trò (Role)
        public async Task AssignRoleAsync(ApplicationUser user, List<string> RoleNames)
        {
            var roles = await _context.Roles
            .Where(r => RoleNames.Contains(r.Name))
            .ToListAsync();

            if (!roles.Any())
                throw new Exception("Không tìm thấy vai trò hợp lệ.");
            var userRoles = roles.Select(role => new IdentityUserRole<string>
            {
                UserId = user.Id,
                RoleId = role.Id
            }).ToList();
            _context.UserRoles.AddRange(userRoles);

        }

        public async Task AssignRoleAsync(ApplicationUser user, string RoleName)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(x => x.Name == RoleName);

            if (role == null)
                throw new Exception("Không tìm thấy vai trò hợp lệ.");
            var userRoles = new IdentityUserRole<string>
            {
                UserId = user.Id,
                RoleId = role.Id
            };
            _context.UserRoles.Add(userRoles);
        }

        public async Task RemoveRoleFromUserAsync(ApplicationUser user, List<string> RoleNames)
        {
            if (RoleNames.Contains("SuperAdmin"))
            {
                var superAdminCount = await _context.UserRoles
                    .Join(_context.Roles,
                          ur => ur.RoleId,
                          r => r.Id,
                          (ur, r) => new { ur.UserId, RoleName = r.Name })
                    .CountAsync(x => x.RoleName == "SuperAdmin");
                if (superAdminCount <= 1)
                {
                    throw new InvalidOperationException("Hệ thống cần ít nhất một SuperAdmin.");
                }
            }

            var rolesToRemove = await _context.Roles
            .Where(r => RoleNames.Contains(r.Name))
            .Select(r => r.Id)
            .ToListAsync();

            var userRoles = await _context.UserRoles
                .Where(ur => ur.UserId == user.Id && rolesToRemove.Contains(ur.RoleId))
                .ToListAsync();

            if (userRoles.Any())
            {
                _context.UserRoles.RemoveRange(userRoles);
            }
        }

        public async Task<IList<string>> GetUserRolesAsync(ApplicationUser user)
        {
            return user != null ? await _userManager.GetRolesAsync(user) : new List<string>();

        }
        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }

        // Quản lý quyền (Claim)

        public async Task<bool> AddClaimToUserAsync(ApplicationUser user, List<Claim> claim)
        {
            var result = await _userManager.AddClaimsAsync(user, claim);
            return result.Succeeded;
        }

        public async Task<bool> RemoveClaimFromUserAsync(ApplicationUser user, List<Claim> claim)
        {
            var result = await _userManager.RemoveClaimsAsync(user, claim);
            return result.Succeeded;
        }

        public async Task<IList<Claim>> GetUserClaimsAsync(ApplicationUser user)
        {
            return user != null ? (await _userManager.GetClaimsAsync(user)).ToList() : new List<Claim>();
        }

        public async Task<bool> UserHasClaimAsync(ApplicationUser user, Claim claim)
        {
            var claims = await _userManager.GetClaimsAsync(user);
            return claims.Any(c => c.Type == claim.Type && c.Value == claim.Value);
        }

        // Quản lý vai trò với claim
        public async Task AddClaimToRoleAsync(string roleName, List<Claim> claims)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null) throw new KeyNotFoundException($"Role '{roleName}' không tồn tại.");

            // Lấy danh sách Claims hiện tại của Role
            var existingClaims = await _roleManager.GetClaimsAsync(role);

            var newClaims = claims.Where(c => !existingClaims.Any(ec => ec.Type == c.Type && ec.Value == c.Value)).ToList();

            if (!newClaims.Any()) throw new InvalidOperationException("Không có claims hợp lệ để thêm.");

            var roleClaims = newClaims.Select(claim => new IdentityRoleClaim<string>
            {
                RoleId = role.Id,
                ClaimType = claim.Type,
                ClaimValue = claim.Value
            }).ToList();

            await _context.RoleClaims.AddRangeAsync(roleClaims);
        }

        public async Task RemoveClaimFromRoleAsync(string roleName, List<Claim> claims)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null) throw new KeyNotFoundException($"Role '{roleName}' không tồn tại.");

            // Lấy danh sách Claims hiện tại của Role
            var existingClaims = await _roleManager.GetClaimsAsync(role);
            var claimsToRemove = existingClaims.Where(c => claims.Any(ec => ec.Type == c.Type && ec.Value == c.Value)).ToList();

            if (!claimsToRemove.Any()) throw new InvalidOperationException("Không có claims hợp lệ để xóa.");

            var roleClaims = await _context.RoleClaims
                .Where(rc => rc.RoleId == role.Id && claims.Any(c => rc.ClaimType == c.Type && rc.ClaimValue == c.Value))
                .ToListAsync();

            _context.RoleClaims.RemoveRange(roleClaims);
        }

        public async Task<IList<Claim>> GetRoleClaimsAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            return role != null ? (await _roleManager.GetClaimsAsync(role)).ToList() : new List<Claim>();
        }

        public async Task<bool> RoleHasClaimAsync(string roleName, Claim claim)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null) return false;
            var claims = await _roleManager.GetClaimsAsync(role);
            return claims.Any(c => c.Type == claim.Type && c.Value == claim.Value);
        }

        public async Task<bool> AddRoleAsync(string roleName)
        {
            if (await _roleManager.RoleExistsAsync(roleName))
                throw new Exception("Role đã tồn tại.");
            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }
            return result.Succeeded;
        }

        public async Task<bool> EditRoleAsync(string oldRoleName, string newRoleName)
        {

            var role = await _roleManager.FindByNameAsync(oldRoleName);
            if (role == null) throw new KeyNotFoundException($"Role '{oldRoleName}' không tồn tại.");

            if (oldRoleName == "SuperAdmin") throw new InvalidOperationException("Không thể cập nhật quyền SuperAdmin.");

            role.Name = newRoleName;
            var result = await _roleManager.UpdateAsync(role);
            return result.Succeeded;
        }

        public async Task DeleteRoleAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null) throw new KeyNotFoundException($"Role '{roleName}' không tồn tại.");

            if (roleName == "SuperAdmin") throw new InvalidOperationException("Không thể xóa quyền SuperAdmin.");

            _context.Roles.Remove(role);
        }

        public async Task<IEnumerable<string>>? GetRolesUser(string UserId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == UserId);

            if (user == null)
            {
                return new List<string>();
            }
            var roles = await _context.UserRoles
                 .Where(ur => ur.UserId == user.Id)
                 .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
                 .ToListAsync();
            return roles;
        }
    }
}
