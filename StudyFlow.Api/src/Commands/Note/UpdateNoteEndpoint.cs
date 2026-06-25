using MediatR;
using StudyFlow.Core.Commands.Note.UpdateNote.Request;
using StudyFlow.Core.Results;

namespace StudyFlow.Api.src.Commands.Note
{
    public static class UpdateNoteEndpoint
    {
        public static void UpdateNote(this IEndpointRouteBuilder app)
        {
            app.MapPut("/api/notes/{noteId:int}", async (int noteId, UpdateNoteDto dto, IMediator mediator) =>
            {
                UpdateNoteCommand command = new UpdateNoteCommand(noteId, dto);

                Result<int> result = await mediator.Send(command);

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
            .WithName("UpdateNote")
            .RequireAuthorization();
        }
    }
}
