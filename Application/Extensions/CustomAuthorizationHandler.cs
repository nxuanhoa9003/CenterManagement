using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.InterfacesServices;
namespace Application.Extensions
{
    public class CustomAuthorizationHandler : AuthorizationHandler<CustomRequirement>
    {
        private readonly IAuthorizationServices _authorizationService;
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger<CustomAuthorizationHandler> _logger;

        public CustomAuthorizationHandler(IAuthorizationServices authorizationService, IAuthenticationService authenticationService, ILogger<CustomAuthorizationHandler> logger)
        {
            _authorizationService = authorizationService;
            _authenticationService = authenticationService;
            _logger = logger;
        }

        
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomRequirement requirement)
        {
            bool hasClaim = await IsHasClaim(context.User, context.Resource, requirement);
            if (hasClaim)
            {
                _logger.LogInformation("Truy cập được");
                context.Succeed(requirement);
            }
            else
            {
                _logger.LogError("Không có quyền truy cập");
            }

            // Kiểm tra nếu chưa có handler nào xác nhận quyền truy cập
            if (!context.HasSucceeded)
            {
                _logger.LogError("❌ Chưa có handler nào xác nhận quyền truy cập.");
            }
        }


        // kiểm tra claim từ database
        private async Task<bool> IsHasClaim(ClaimsPrincipal user, object? resource, CustomRequirement requirement)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return false;

            var currentuser = await _authenticationService.GetUserById(userId);
            // Kiểm tra UserClaims
            var userClaims = await _authorizationService.GetUserClaimsAsync(currentuser);
            if (userClaims.Any(c => c.Type == requirement.RequiredClaimType && c.Value == requirement.RequiredClaimValue))
            {
                return true;
            }

            // Kiểm tra RoleClaims
            var roles = await _authorizationService.GetUserRolesAsync(currentuser);
            foreach (var role in roles)
            {
                var roleClaims = await _authorizationService.GetRoleClaimsAsync(role);
                if (roleClaims.Any(c => c.Type == requirement.RequiredClaimType && c.Value == requirement.RequiredClaimValue))
                {
                    return true;
                }
            }

            return false;
        }

    }
}
