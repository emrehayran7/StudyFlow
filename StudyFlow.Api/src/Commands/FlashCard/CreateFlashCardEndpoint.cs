using MediatR;
using StudyFlow.Core.Commands.FlashCard.CreateFlashCard.Request;
using StudyFlow.Core.Results;

namespace StudyFlow.Api.src.Commands.FlashCard
{
    public static class CreateFlashCardEndpoint
    {
        public static void CreateFlashCard(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/flashcards", async (CreateFlashCardDto dto, IMediator mediator) =>
            {
                var command = new CreateFlashCardCommand(dto);

                var result = await mediator.Send(command);

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
            .WithName("CreateFlashCard")
            .RequireAuthorization();
        }
    }
}
