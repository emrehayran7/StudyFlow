using MediatR;
using StudyFlow.Core.Commands.Auth.Request;
using StudyFlow.Core.Results;

namespace StudyFlow.Api.src.Commands.Auth;

public static class LoginEndpoint
{
    public static void Login(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/login", async (LoginDto dto, IMediator mediator) =>
        {
            var result = await mediator.Send(new LoginCommand(dto));

            if (result.IsSuccess)
            {
                return Results.Ok(result.Value);
            }

            return Results.BadRequest(new
            {
                code = result.Error.Code,
                result.Error.Description
            });
        })
        .WithName("Login");
        
    }
}