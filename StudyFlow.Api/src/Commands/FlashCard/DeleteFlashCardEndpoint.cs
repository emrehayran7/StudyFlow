using MediatR;
using StudyFlow.Api.src.Extensions;
using StudyFlow.Core.Commands.FlashCard.DeleteFlashCard.Request;
using StudyFlow.Core.Results;
using System.Security.Claims;

namespace StudyFlow.Api.src.Commands.FlashCard
{
    public static class DeleteFlashCardEndpoint
    {
        public static void DeleteFlashCard(this IEndpointRouteBuilder app)
        {
            app.MapDelete("/api/flashcards/{flashCardId:int}", async (int flashCardId, ClaimsPrincipal user, IMediator mediator) =>
            {
                int userId = user.GetUserId();
                var command = new DeleteFlashCardCommand(flashCardId, userId);

                var result = await mediator.Send(command);

                if (result.IsSuccess)
                {
                    return Results.NoContent();
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
            .WithName("DeleteFlashCard")
            .RequireAuthorization();
        }
    }
}
