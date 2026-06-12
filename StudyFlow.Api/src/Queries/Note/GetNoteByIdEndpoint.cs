using MediatR;
using StudyFlow.Api.src.Extensions;
using StudyFlow.Core.Queries.Note;
using StudyFlow.Core.Results;
using System.Security.Claims;

namespace StudyFlow.Api.src.Queries.Note
{
    public static class GetNoteByIdEndpoint
    {
        public static void GetNoteById(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/notes/{noteId:int}", async (int noteId, ClaimsPrincipal user, IMediator mediator) =>
            {
                int userId = user.GetUserId();
                var result = await mediator.Send(new GetNoteByIdQuery(noteId, userId));

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
