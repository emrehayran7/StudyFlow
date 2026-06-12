using StudyFlow.Core.Helper;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace StudyFlow.Infrastructure.Services.Auth
{
    public class RefreshTokenService : IRefreshTokenService
    {
        public string GenerateRefreshToken()
        {
            byte[] bytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(bytes);
        }

        public string HashRefreshToken(string refreshToken)
        {
            byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken));
            return Convert.ToBase64String(bytes);
        }
    }
}
