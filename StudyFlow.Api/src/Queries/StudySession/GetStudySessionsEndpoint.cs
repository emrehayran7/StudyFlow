using MediatR;
using StudyFlow.Api.src.Extensions;
using StudyFlow.Core.Queries.StudySession.Response;
using StudyFlow.Core.Results;
using System.Security.Claims;

namespace StudyFlow.Api.src.Queries.StudySession
{
    public static class GetStudySessionsEndpoint
    {
        public static void GetStudySessions(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/studysessions", async (ClaimsPrincipal user, IMediator mediator) =>
            {
                int userId = user.GetUserId();
                var result = await mediator.Send(new GetStudySessionsQuery(userId));

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
