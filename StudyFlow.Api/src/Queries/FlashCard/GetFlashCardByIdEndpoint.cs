using MediatR;
using StudyFlow.Api.src.Extensions;
using StudyFlow.Core.Queries.FlashCard;
using StudyFlow.Core.Results;
using System.Security.Claims;

namespace StudyFlow.Api.src.Queries.FlashCard
{
    public static class GetFlashCardByIdEndpoint
    {
        public static void GetFlashCardById(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/flashcards/{flashCardId:int}", async (int flashCardId, ClaimsPrincipal user, IMediator mediator) =>
            {
                int userId = user.GetUserId();
                var query = new GetFlashCardByIdQuery(flashCardId, userId);

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
