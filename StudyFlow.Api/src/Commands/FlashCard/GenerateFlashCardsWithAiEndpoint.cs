using MediatR;
using StudyFlow.Api.src.Extensions;
using StudyFlow.Core.Commands.FlashCard.GenerateFlashCardsWithAi.Request;
using StudyFlow.Core.Results;
using System.Security.Claims;

namespace StudyFlow.Api.src.Commands.FlashCard
{
    public static class GenerateFlashCardsWithAiEndpoint
    {
        public static void GenerateFlashCardsWithAi(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/flashcards/generate-ai", async (GenerateFlashCardsWithAiDto dto, ClaimsPrincipal user, IMediator mediator) =>
            {
                int userId = user.GetUserId();
                var result = await mediator.Send(new GenerateFlashCardsWithAiCommand(dto, userId));

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
