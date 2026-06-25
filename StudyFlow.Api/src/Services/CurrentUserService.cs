using System.Security.Claims;
using StudyFlow.Core.Helper;

namespace StudyFlow.Api.src.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetUserId()
        {
            ClaimsPrincipal? user = _httpContextAccessor.HttpContext?.User;
            string? userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userId, out int parsedUserId))
            {
                throw new UnauthorizedAccessException("User id claim was not found.");
            }

            return parsedUserId;
        }
    }
}
