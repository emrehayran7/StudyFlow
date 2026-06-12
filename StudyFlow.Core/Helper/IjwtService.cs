using StudyFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudyFlow.Core.Helper
{
    public interface IJwtService
    {
        string GenerateAccessToken(User user, out DateTime expiresAt);
    }
}
