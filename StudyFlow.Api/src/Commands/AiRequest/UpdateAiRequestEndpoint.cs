using MediatR;
using StudyFlow.Api.src.Extensions;
using StudyFlow.Core.Commands.AiRequest.UpdateAiRequest.Request;
using StudyFlow.Core.Results;
using System.Security.Claims;

namespace StudyFlow.Api.src.Commands.AiRequest
{
    public static class UpdateAiRequestEndpoint
    {
        public static void UpdateAiRequest(this IEndpointRouteBuilder app)
        {
            app.MapPut("/api/airequests/{aiRequestId:int}", async (int aiRequestId, UpdateAiRequestDto dto, ClaimsPrincipal user, IMediator mediator) =>
            {
                int userId = user.GetUserId();
                var result = await mediator.Send(new UpdateAiRequestCommand(aiRequestId, dto, userId));

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
            .WithName("UpdateAiRequest")
            .RequireAuthorization();
        }
    }
}
