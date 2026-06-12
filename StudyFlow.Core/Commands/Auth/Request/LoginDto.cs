using System;
using System.Collections.Generic;
using System.Text;

namespace StudyFlow.Core.Commands.Auth.Request
{
    public class LoginDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
