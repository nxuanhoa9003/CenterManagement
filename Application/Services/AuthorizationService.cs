using Application.InterfacesServices;
using Domain.Entities;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AuthorizationService : IAuthorizationServices
    {
        private readonly IAuthorizationRepository _authorizationRepository;

        public AuthorizationService(IAuthorizationRepository authorizationRepository)
        {
            _authorizationRepository = authorizationRepository;
        }

        // Quản lý vai trò (Role)
        public async Task AssignRoleAsync(ApplicationUser user, List<string> RoleNames)
        {
            await _authorizationRepository.AssignRoleAsync(user, RoleNames);
        }
        public async Task AssignRoleAsync(ApplicationUser user, string RoleName)
        {
            await _authorizationRepository.AssignRoleAsync(user, RoleName);
        }

        public async Task RemoveRoleFromUserAsync(ApplicationUser user, List<string> RoleNames)
        {
            await _authorizationRepository.RemoveRoleFromUserAsync(user, RoleNames);
        }

        public async Task<IList<string>> GetUserRolesAsync(ApplicationUser user)
        {
            return await _authorizationRepository.GetUserRolesAsync(user);
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _authorizationRepository.RoleExistsAsync(roleName);
        }

        // Quản lý quyền (Claim)
        public async Task<bool> AddClaimToUserAsync(ApplicationUser user, List<Claim> claim)
        {
            return await _authorizationRepository.AddClaimToUserAsync(user, claim);
        }

        public async Task<bool> RemoveClaimFromUserAsync(ApplicationUser user, List<Claim> claim)
        {
            return await _authorizationRepository.RemoveClaimFromUserAsync(user, claim);
        }

        public async Task<IList<Claim>> GetUserClaimsAsync(ApplicationUser user)
        {
            return await _authorizationRepository.GetUserClaimsAsync(user);
        }

        public async Task<bool> UserHasClaimAsync(ApplicationUser user, Claim claim)
        {
            return await _authorizationRepository.UserHasClaimAsync(user, claim);
        }

        // Quản lý vai trò với claim
        public async Task AddClaimToRoleAsync(string roleName, List<Claim> claims)
        {
            await _authorizationRepository.AddClaimToRoleAsync(roleName, claims);
        }

        public async Task RemoveClaimFromRoleAsync(string roleName, List<Claim> claims)
        {
            await _authorizationRepository.RemoveClaimFromRoleAsync(roleName, claims);
        }

        public async Task<IList<Claim>> GetRoleClaimsAsync(string roleName)
        {
            return await _authorizationRepository.GetRoleClaimsAsync(roleName);
        }

        public async Task<bool> RoleHasClaimAsync(string roleName, Claim claim)
        {
            return await _authorizationRepository.RoleHasClaimAsync(roleName, claim);
        }

        public async Task<bool> AddRoleAsync(string roleName)
        {
            return await _authorizationRepository.AddRoleAsync(roleName);
        }

        public async Task DeleteRoleAsync(string roleName)
        {
            await _authorizationRepository.DeleteRoleAsync(roleName);
        }

        public async Task<bool> EditRoleAsync(string oldRoleName, string newRoleName)
        {
            return await _authorizationRepository.EditRoleAsync(oldRoleName, newRoleName);
        }
    }
}
