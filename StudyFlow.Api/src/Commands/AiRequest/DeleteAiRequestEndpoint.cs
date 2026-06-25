using MediatR;
using StudyFlow.Core.Commands.AiRequest.DeleteAiRequest.Request;
using StudyFlow.Core.Results;

namespace StudyFlow.Api.src.Commands.AiRequest
{
    public static class DeleteAiRequestEndpoint
    {
        public static void DeleteAiRequest(this IEndpointRouteBuilder app)
        {
            app.MapDelete("/api/airequests/{aiRequestId:int}", async (int aiRequestId, IMediator mediator) =>
            {
                var result = await mediator.Send(new DeleteAiRequestCommand(aiRequestId));

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
