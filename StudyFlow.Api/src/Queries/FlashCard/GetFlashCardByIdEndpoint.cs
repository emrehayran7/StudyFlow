using MediatR;
using StudyFlow.Core.Queries.FlashCard;
using StudyFlow.Core.Results;

namespace StudyFlow.Api.src.Queries.FlashCard
{
    public static class GetFlashCardByIdEndpoint
    {
        public static void GetFlashCardById(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/flashcards/{flashCardId:int}", async (int flashCardId, IMediator mediator) =>
            {
                var query = new GetFlashCardByIdQuery(flashCardId);

                var result = await mediator.Send(query);

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
            .WithName("GetFlashCardById")
            .RequireAuthorization();
        }
    }
}
