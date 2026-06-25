using MediatR;
using StudyFlow.Core.Commands.StudySession.CreateStudySession.Request;
using StudyFlow.Core.Results;

namespace StudyFlow.Api.src.Commands.StudySession
{
    public static class CreateStudySessionEndpoint
    {
        public static void CreateStudySession(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/studysessions", async (CreateStudySessionDto dto, IMediator mediator) =>
            {
                var command = new CreateStudySessionCommand(dto);

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
            .WithName("CreateStudySession")
            .RequireAuthorization();
        }
    }
}
