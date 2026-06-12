using MediatR;
using StudyFlow.Api.src.Extensions;
using StudyFlow.Core.Queries.Topic.Response;
using StudyFlow.Core.Results;
using System.Security.Claims;

namespace StudyFlow.Api.src.Queries.Topic
{
    public static class GetTopicsEndpoint
    {
        public static void GetTopics(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/topics", async (int courseId, ClaimsPrincipal user, IMediator mediator) =>
            {
                int userId = user.GetUserId();
                var result = await mediator.Send(new GetTopicsQuery(courseId, userId));

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
            .WithName("GetTopics")
            .RequireAuthorization();
        }
    }
}
