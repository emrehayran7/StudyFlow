using MediatR;
using StudyFlow.Core.Commands.FlashCard.DeleteFlashCard.Request;
using StudyFlow.Core.Results;

namespace StudyFlow.Api.src.Commands.FlashCard
{
    public static class DeleteFlashCardEndpoint
    {
        public static void DeleteFlashCard(this IEndpointRouteBuilder app)
        {
            app.MapDelete("/api/flashcards/{flashCardId:int}", async (int flashCardId, IMediator mediator) =>
            {
                var command = new DeleteFlashCardCommand(flashCardId);

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
