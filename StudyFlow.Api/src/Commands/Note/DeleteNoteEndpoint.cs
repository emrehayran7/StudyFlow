using MediatR;
using StudyFlow.Core.Commands.Note.DeleteNote.Request;
using StudyFlow.Core.Results;

namespace StudyFlow.Api.src.Commands.Note
{
    public static class DeleteNoteEndpoint
    {
        public static void DeleteNote(this IEndpointRouteBuilder app)
        {
            app.MapDelete("/api/notes/{noteId:int}", async (int noteId, IMediator mediator) =>
            {
                var command = new DeleteNoteCommand(noteId);

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
            .WithName("DeleteNote")
            .RequireAuthorization();
        }
    }
}
