using MediatR;
using StudyFlow.Core.Commands.AiRequest.CreateAiRequest.Request;
using StudyFlow.Core.Results;

namespace StudyFlow.Api.src.Commands.AiRequest
{
    public static class CreateAiRequestEndpoint
    {
        public static void CreateAiRequest(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/airequests", async (CreateAiRequestDto dto, IMediator mediator) =>
            {
                var result = await mediator.Send(new CreateAiRequestCommand(dto));

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
            .WithName("CreateAiRequest")
            .RequireAuthorization();
        }
    }
}
