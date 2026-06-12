using MediatR;
using StudyFlow.Api.src.Extensions;
using StudyFlow.Core.Queries.FlashCard.Response;
using StudyFlow.Core.Results;
using System.Security.Claims;

namespace StudyFlow.Api.src.Queries.FlashCard
{
    public static class GetFlashCardsEndpoint
    {
        public static void GetFlashCards(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/flashcards", async (int topicId, ClaimsPrincipal user, IMediator mediator) =>
            {
                int userId = user.GetUserId();
                var result = await mediator.Send(new GetFlashCardsQuery(topicId, userId));

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
            .WithName("GetFlashCards")
            .RequireAuthorization();
        }
    }
}
