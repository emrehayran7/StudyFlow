using MediatR;
using StudyFlow.Core.Commands.StudySession.UpdateStudySession.Request;
using StudyFlow.Core.Results;

namespace StudyFlow.Api.src.Commands.StudySession
{
    public static class UpdateStudySessionEndpoint
    {
        public static void UpdateStudySession(this IEndpointRouteBuilder app)
        {
            app.MapPut("/api/studysessions/{id:int}", async (int id, UpdateStudySessionDto dto, IMediator mediator) =>
            {
                var command = new UpdateStudySessionCommand(id, dto);

                var result = await mediator.Send(command);

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
            .WithName("UpdateStudySession")
            .RequireAuthorization();
        }
    }
}
