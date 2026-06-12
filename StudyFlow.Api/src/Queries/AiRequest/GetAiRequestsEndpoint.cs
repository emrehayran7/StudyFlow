using MediatR;
using StudyFlow.Api.src.Extensions;
using StudyFlow.Core.Queries.AiRequest.Response;
using StudyFlow.Core.Results;
using System.Security.Claims;

namespace StudyFlow.Api.src.Queries.AiRequest
{
    public static class GetAiRequestsEndpoint
    {
        public static void GetAiRequests(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/airequests", async (ClaimsPrincipal user, IMediator mediator) =>
            {
                int userId = user.GetUserId();
                var result = await mediator.Send(new GetAiRequestsQuery(userId));

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
            .WithName("GetAiRequests")
            .RequireAuthorization();
        }
    }
}
