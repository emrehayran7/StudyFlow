using MediatR;
using StudyFlow.Api.src.Extensions;
using StudyFlow.Core.Commands.FlashCard.UpdateFlashCard.Request;
using StudyFlow.Core.Results;
using System.Security.Claims;

namespace StudyFlow.Api.src.Commands.FlashCard
{
    public static class UpdateFlashCardEndpoint
    {
        public static void UpdateFlashCard(this IEndpointRouteBuilder app)
        {
            app.MapPut("/api/flashcards/{flashCardId:int}", async (int flashCardId, UpdateFlashCardDto dto, ClaimsPrincipal user, IMediator mediator) =>
            {
                int userId = user.GetUserId();
                var command = new UpdateFlashCardCommand(flashCardId, dto, userId);

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
            .WithName("UpdateFlashCard")
            .RequireAuthorization();
        }
    }
}
