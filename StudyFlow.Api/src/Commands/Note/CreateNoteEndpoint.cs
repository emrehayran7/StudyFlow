using MediatR;
using StudyFlow.Api.src.Extensions;
using StudyFlow.Core.Commands.Note.CreateNote.Request;
using StudyFlow.Core.Results;
using System.Security.Claims;

namespace StudyFlow.Api.src.Commands.Note
{
    public static class CreateNoteEndpoint
    {
        public static void CreateNote(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/notes", async (CreateNoteDto dto, ClaimsPrincipal user, IMediator mediator) =>
            {
                int userId = user.GetUserId();
                CreateNoteCommand command = new CreateNoteCommand(dto, userId);

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
            .WithName("CreateNote")
            .RequireAuthorization();
        }
    }
}
