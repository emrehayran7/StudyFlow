using MediatR;
using StudyFlow.Core.Commands.Auth.Register.Request;
using StudyFlow.Core.Results;

namespace StudyFlow.Api.src.Commands.Auth;

public static class RegisterEndpoint
{
    public static void Register(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/register", async (RegisterDto dto, IMediator mediator) =>
        {
            var result = await mediator.Send(new RegisterCommand(dto));

            if (result.IsSuccess)
            {
                return Results.Ok(result.Value);
            }

            return result.Error.Type == ErrorType.Conflict
                ? Results.Conflict(new
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
        .WithName("Register");
    }
}
