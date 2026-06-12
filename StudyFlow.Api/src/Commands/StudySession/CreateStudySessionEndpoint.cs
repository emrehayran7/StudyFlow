using MediatR;
using StudyFlow.Api.src.Extensions;
using StudyFlow.Core.Commands.StudySession.CreateStudySession.Request;
using StudyFlow.Core.Results;
using System.Security.Claims;

namespace StudyFlow.Api.src.Commands.StudySession
{
    public static class CreateStudySessionEndpoint
    {
        public static void CreateStudySession(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/studysessions", async (CreateStudySessionDto dto, ClaimsPrincipal user, IMediator mediator) =>
            {
                int userId = user.GetUserId();
                var command = new CreateStudySessionCommand(dto, userId);

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
