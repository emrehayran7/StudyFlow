using MediatR;
using StudyFlow.Api.src.Extensions;
using StudyFlow.Core.Commands.FlashCard.SaveGeneratedFlashCards.Request;
using StudyFlow.Core.Results;
using System.Security.Claims;

namespace StudyFlow.Api.src.Commands.FlashCard
{
    public static class SaveGeneratedFlashCardsEndpoint
    {
        public static void SaveGeneratedFlashCards(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/flashcards/generated-ai/save", async (SaveGeneratedFlashCardsDto dto, ClaimsPrincipal user, IMediator mediator) =>
            {
                int userId = user.GetUserId();
                var command = new SaveGeneratedFlashCardsCommand(dto, userId);

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
            .WithName("SaveGeneratedFlashCards")
            .RequireAuthorization();
        }
    }
}
