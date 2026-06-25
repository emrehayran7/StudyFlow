using MediatR;
using StudyFlow.Core.Commands.AiRequest.UpdateAiRequest.Request;
using StudyFlow.Core.Results;

namespace StudyFlow.Api.src.Commands.AiRequest
{
    public static class UpdateAiRequestEndpoint
    {
        public static void UpdateAiRequest(this IEndpointRouteBuilder app)
        {
            app.MapPut("/api/airequests/{aiRequestId:int}", async (int aiRequestId, UpdateAiRequestDto dto, IMediator mediator) =>
            {
                var result = await mediator.Send(new UpdateAiRequestCommand(aiRequestId, dto));

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
