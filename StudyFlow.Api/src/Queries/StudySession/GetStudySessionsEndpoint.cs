using MediatR;
using StudyFlow.Core.Queries.StudySession.Response;
using StudyFlow.Core.Results;

namespace StudyFlow.Api.src.Queries.StudySession
{
    public static class GetStudySessionsEndpoint
    {
        public static void GetStudySessions(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/studysessions", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetStudySessionsQuery());

                if (result.IsSuccess)
                {
                    return Results.Ok(result.Value);
                }

                return result.Error.Type == ErrorType.NotFound
                    ? Results.NotFound(new
                    {
                        code = result.Error.Code,
                        result.Error.Description
                    })
                    : Results.BadRequest(new
                    {
                        code = result.Error.Code,
                        result.Error.Description
                    });
            })
            .WithName("GetStudySessions")
            .RequireAuthorization();
        }
    }
}
