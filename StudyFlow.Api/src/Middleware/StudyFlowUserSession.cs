namespace StudyFlow.Api.src.Middleware
{
    public sealed record StudyFlowUserSession(
        bool IsAuthenticated,
        int? UserId,
        string? Email,
        IReadOnlyList<string> Roles,
        string CorrelationId,
        string SessionId);
}
