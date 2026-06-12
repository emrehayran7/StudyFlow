using MediatR;
using StudyFlow.Api.src.Extensions;
using StudyFlow.Core.Commands.Note.UpdateNote.Request;
using StudyFlow.Core.Results;
using System.Security.Claims;

namespace StudyFlow.Api.src.Commands.Note
{
    public static class UpdateNoteEndpoint
    {
        public static void UpdateNote(this IEndpointRouteBuilder app)
        {
            app.MapPut("/api/notes/{noteId:int}", async (int noteId, UpdateNoteDto dto, ClaimsPrincipal user, IMediator mediator) =>
            {
                int userId = user.GetUserId();
                UpdateNoteCommand command = new UpdateNoteCommand(noteId, dto, userId);

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
