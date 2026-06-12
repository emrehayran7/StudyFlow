using MediatR;
using StudyFlow.Api.src.Extensions;
using StudyFlow.Core.Queries.StudySession;
using StudyFlow.Core.Results;
using System.Security.Claims;

namespace StudyFlow.Api.src.Queries.StudySession
{
    public static class GetStudySessionByIdEndpoint
    {
        public static void GetStudySessionById(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/studysessions/{id:int}", async (int id, ClaimsPrincipal user, IMediator mediator) =>
            {
                int userId = user.GetUserId();
                var query = new GetStudySessionByIdQuery(id, userId);

                var result = await mediator.Send(query);

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
            .WithName("GetStudySessionById")
            .RequireAuthorization();
        }
    }
}
