using Domain.Entities;
using Shared.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.InterfacesServices
{
    public interface ITokenService
    {
        public Task<(string Token, string AccessTokenId)> GenerateToken(LoginResponse user);
        string GenerateRefreshToken();
        Task SaveRefreshTokenAsync(string userId, string refreshToken, string accessTokenId);
        Task<RefreshToken?> GetRefreshTokenAsync(string token);
        ClaimsPrincipal? ValidateAccessToken(string accessToken);
        Task UpdateTokenUser(RefreshToken token);
        Task RevokeAllRefreshTokenAsync(string userId);
    }
}
