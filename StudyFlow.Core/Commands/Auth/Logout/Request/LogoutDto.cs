using System;
using System.Collections.Generic;
using System.Text;

namespace StudyFlow.Core.Commands.Auth.Logout.Request
{
    public class LogoutDto
    {
        public string RefreshToken { get; set; } = null!;
    }
}
