using MediatR;
using StudyFlow.Core.Commands.Auth.Logout.Request;
using StudyFlow.Core.Results;

namespace StudyFlow.Api.src.Commands.Auth;

public static class LogoutEndpoint
{
    public static void Logout(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/logout", async (LogoutDto dto, IMediator mediator) =>
        {
            var result = await mediator.Send(new LogoutCommand(dto));

            if (result.IsSuccess)
            {
                return Results.Ok();
            }

            return Results.BadRequest(new
            {
                code = result.Error.Code,
                result.Error.Description
            });
        })
        .WithName("Logout");
    }
}