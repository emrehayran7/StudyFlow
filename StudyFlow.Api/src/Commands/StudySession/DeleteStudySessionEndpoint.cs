using MediatR;
using StudyFlow.Api.src.Extensions;
using StudyFlow.Core.Commands.StudySession.DeleteStudySession.Request;
using StudyFlow.Core.Results;
using System.Security.Claims;

namespace StudyFlow.Api.src.Commands.StudySession
{
    public static class DeleteStudySessionEndpoint
    {
        public static void DeleteStudySession(this IEndpointRouteBuilder app)
        {
            app.MapDelete("/api/studysessions/{id:int}", async (int id, ClaimsPrincipal user, IMediator mediator) =>
            {
                int userId = user.GetUserId();
                var command = new DeleteStudySessionCommand(id, userId);

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
            .WithName("DeleteStudySession")
            .RequireAuthorization();
        }
    }
}
