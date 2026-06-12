using MediatR;
using StudyFlow.Api.src.Extensions;
using StudyFlow.Core.Commands.Note.DeleteNote.Request;
using StudyFlow.Core.Results;
using System.Security.Claims;

namespace StudyFlow.Api.src.Commands.Note
{
    public static class DeleteNoteEndpoint
    {
        public static void DeleteNote(this IEndpointRouteBuilder app)
        {
            app.MapDelete("/api/notes/{noteId:int}", async (int noteId, ClaimsPrincipal user, IMediator mediator) =>
            {
                int userId = user.GetUserId();
                var command = new DeleteNoteCommand(noteId, userId);

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
