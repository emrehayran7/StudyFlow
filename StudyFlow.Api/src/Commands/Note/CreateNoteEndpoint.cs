using MediatR;
using StudyFlow.Core.Commands.Note.CreateNote.Request;
using StudyFlow.Core.Results;

namespace StudyFlow.Api.src.Commands.Note
{
    public static class CreateNoteEndpoint
    {
        public static void CreateNote(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/notes", async (CreateNoteDto dto, IMediator mediator) =>
            {
                CreateNoteCommand command = new CreateNoteCommand(dto);

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
