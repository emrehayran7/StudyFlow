using System;
using System.Collections.Generic;
using System.Text;

namespace StudyFlow.Core.Helper
{
    public interface IPasswordHasherService
    {
        byte[] HashPassword(string password);
        bool VerifyPassword(string password, byte[] passwordHash);
    }
}
