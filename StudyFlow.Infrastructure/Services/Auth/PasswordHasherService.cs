using StudyFlow.Core.Helper;
using System.Text;

namespace StudyFlow.Infrastructure.Services.Auth;

public class PasswordHasherService : IPasswordHasherService
{
    public byte[] HashPassword(string password)
    {
        var hashString = BCrypt.Net.BCrypt.HashPassword(password);

        return Encoding.UTF8.GetBytes(hashString);
    }

    public bool VerifyPassword(string password, byte[] passwordHash)
    {
        if (string.IsNullOrWhiteSpace(password) || passwordHash is null || passwordHash.Length == 0)
        {
            return false;
        }

        try
        {
            var hashString = Encoding.UTF8.GetString(passwordHash);

            return BCrypt.Net.BCrypt.Verify(password, hashString);
        }
        catch (BCrypt.Net.SaltParseException)
        {
            return false;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}