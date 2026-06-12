using MediatR;
using StudyFlow.Api.src.Extensions;
using StudyFlow.Core.Commands.AiRequest.DeleteAiRequest.Request;
using StudyFlow.Core.Results;
using System.Security.Claims;

namespace StudyFlow.Api.src.Commands.AiRequest
{
    public static class DeleteAiRequestEndpoint
    {
        public static void DeleteAiRequest(this IEndpointRouteBuilder app)
        {
            app.MapDelete("/api/airequests/{aiRequestId:int}", async (int aiRequestId, ClaimsPrincipal user, IMediator mediator) =>
            {
                int userId = user.GetUserId();
                var result = await mediator.Send(new DeleteAiRequestCommand(aiRequestId, userId));

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
            .WithName("DeleteAiRequest")
            .RequireAuthorization();
        }
    }
}
