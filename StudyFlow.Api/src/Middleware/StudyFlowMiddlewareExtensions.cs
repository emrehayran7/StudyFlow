namespace StudyFlow.Api.src.Middleware
{
    public static class StudyFlowMiddlewareExtensions
    {
        public static IApplicationBuilder UseStudyFlowRequestMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<StudyFlowRequestMiddleware>();
        }
    }
}
