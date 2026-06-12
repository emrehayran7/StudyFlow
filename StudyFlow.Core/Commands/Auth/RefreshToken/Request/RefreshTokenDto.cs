using System;
using System.Collections.Generic;
using System.Text;

namespace StudyFlow.Core.Commands.Auth.RefreshToken.Request
{
    public class RefreshTokenDto
    {
        public string RefreshToken { get; set; } = null!;
    }
}
