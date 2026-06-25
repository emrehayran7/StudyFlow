using MediatR;
using StudyFlow.Core.Queries.Note;
using StudyFlow.Core.Results;

namespace StudyFlow.Api.src.Queries.Note
{
    public static class GetNoteByIdEndpoint
    {
        public static void GetNoteById(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/notes/{noteId:int}", async (int noteId, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetNoteByIdQuery(noteId));

                if (result.IsSuccess)
                {
                    return Results.Ok(result.Value);
                }

                return Results.NotFound(new
                {
                    code = result.Error.Code,
                    result.Error.Description
                });
            })
            .WithName("GetNoteById")
            .RequireAuthorization();
        }
    }
}
