using MediatR;
using StudyFlow.Core.Queries.Note.Response;
using StudyFlow.Core.Results;

namespace StudyFlow.Api.src.Queries.Note
{
    public static class GetNotesEndpoint
    {
        public static void GetNotes(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/notes", async (int topicId, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetNotesQuery(topicId));

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
            .WithName("GetNotes")
            .RequireAuthorization();
        }
    }
}
