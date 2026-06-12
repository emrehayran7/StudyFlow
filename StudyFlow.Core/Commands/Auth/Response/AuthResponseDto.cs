using System;
using System.Collections.Generic;
using System.Text;

namespace StudyFlow.Core.Commands.Auth.Response
{
    public class AuthResponseDto
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public DateTime AccessTokenExpiresAt { get; set; }
    }
}
