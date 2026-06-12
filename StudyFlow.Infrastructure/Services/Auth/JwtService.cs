using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StudyFlow.Core.Auth;
using StudyFlow.Core.Helper;
using StudyFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace StudyFlow.Infrastructure.Services.Auth
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _settings;
          
        public JwtService(IOptions<JwtSettings> settings)
        {
            _settings = settings.Value;
        }
        
        public string GenerateAccessToken(User user, out DateTime expiresAt)
        {
            expiresAt = DateTime.UtcNow.AddMinutes(_settings.AccessTokenExpirationMinutes);
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            foreach (var userRole in user.UserRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole.Role.Name));
            }

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        
    }
}
