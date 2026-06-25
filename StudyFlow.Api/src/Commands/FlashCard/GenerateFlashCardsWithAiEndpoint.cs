using MediatR;
using StudyFlow.Core.Commands.FlashCard.GenerateFlashCardsWithAi.Request;
using StudyFlow.Core.Results;

namespace StudyFlow.Api.src.Commands.FlashCard
{
    public static class GenerateFlashCardsWithAiEndpoint
    {
        public static void GenerateFlashCardsWithAi(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/flashcards/generate-ai", async (GenerateFlashCardsWithAiDto dto, IMediator mediator) =>
            {
                var result = await mediator.Send(new GenerateFlashCardsWithAiCommand(dto));

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
            .WithName("GenerateFlashCardsWithAi")
            .RequireAuthorization();
        }
    }
}
