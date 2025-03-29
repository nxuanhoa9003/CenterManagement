using Domain.Entities;
using Domain.Interface;
using Application.InterfacesServices;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shared.DTOs.Auth;
using Shared.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IAuthorizationRepository _authorizationRepository;

        public TokenService(JwtSettings jwtSettings, IRefreshTokenRepository refreshTokenRepository, IAuthorizationRepository authorizationRepository)
        {
            _jwtSettings = jwtSettings;
            _refreshTokenRepository = refreshTokenRepository;
            _authorizationRepository = authorizationRepository;
        }

        #region access token
        public async Task<(string Token, string AccessTokenId)> GenerateToken(LoginResponse user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId ?? ""),  // Subject
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique ID
                new Claim(ClaimTypes.NameIdentifier, user.UserId),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("FullName", user.FullName),

            };

            var roles = (await _authorizationRepository.GetRolesUser(user.UserId))?.ToList() ?? new List<string>();

            // Thêm Role vào Claims nếu có
            if (roles != null)
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var accessTokenId = token.Id;
            return (new JwtSecurityTokenHandler().WriteToken(token), accessTokenId);
        }
        #endregion access token


        #region refresh token
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task SaveRefreshTokenAsync(string userId, string refreshToken, string accessTokenId)
        {
            var newfreshToken = new RefreshToken
            {
                UserId = userId,
                AccessTokenId = accessTokenId,
                Token = refreshToken,
                ExpiryDate = DateTime.Now.AddDays(7),
            };
            await _refreshTokenRepository.AddToken(newfreshToken);
        }

        public async Task<RefreshToken?> GetRefreshTokenAsync(string token)
        {
            return await _refreshTokenRepository.GetToken(token);
        }
        #endregion refresh token



        #region ValidateAccessToken 
        public ClaimsPrincipal? ValidateAccessToken(string accessToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidAudience = _jwtSettings.Audience,
                    ValidateLifetime = false
                };


                return new ClaimsPrincipal(tokenHandler.ValidateToken(accessToken, validationParameters, out _));
            }
            catch
            {
                return null; // Token không hợp lệ
            }
        }

        #endregion ValidateAccessToken



        public async Task UpdateTokenUser(RefreshToken token)
        {
            await _refreshTokenRepository.UpdateToken(token);
        }

        // xoá hết token khi thay đổi mật khẩu
        public async Task RevokeAllRefreshTokenAsync(string userId)
        {
            await _refreshTokenRepository.DeleteTokeByUserId(userId);
        }


    }
}
