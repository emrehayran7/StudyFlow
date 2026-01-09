using StudyFlow.src.Application.Abstractions;

namespace StudyFlow.src.Application
{
    public class HealthService : IHealthService
    {
        // Simple health check implementation
        public object GetHealth() => new
        {
            status = "ok",
            service = "StudyFlow",
            utc = DateTime.UtcNow
        };
    }
}
