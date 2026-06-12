using System.Security.Claims;

namespace StudyFlow.Api.src.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            string? userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UnauthorizedAccessException("User id claim was not found.");
            }

            return int.Parse(userId);
        }
    }
}
