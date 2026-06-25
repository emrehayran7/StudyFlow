using MediatR;
using StudyFlow.Core.Commands.StudySession.DeleteStudySession.Request;
using StudyFlow.Core.Results;

namespace StudyFlow.Api.src.Commands.StudySession
{
    public static class DeleteStudySessionEndpoint
    {
        public static void DeleteStudySession(this IEndpointRouteBuilder app)
        {
            app.MapDelete("/api/studysessions/{id:int}", async (int id, IMediator mediator) =>
            {
                var command = new DeleteStudySessionCommand(id);

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
