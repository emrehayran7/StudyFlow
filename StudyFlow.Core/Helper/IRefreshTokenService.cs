using System;
using System.Collections.Generic;
using System.Text;

namespace StudyFlow.Core.Helper
{
    public interface IRefreshTokenService
    {
        string GenerateRefreshToken();
        string HashRefreshToken(string refreshToken);
    }
}
