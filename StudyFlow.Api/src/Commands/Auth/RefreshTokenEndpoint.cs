using MediatR;
using StudyFlow.Core.Commands.Auth.RefreshToken.Request;
using StudyFlow.Core.Results;

namespace StudyFlow.Api.src.Commands.Auth;

public static class RefreshTokenEndpoint
{
    public static void RefreshToken(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/refresh", async (RefreshTokenDto dto, IMediator mediator) =>
        {
            var result = await mediator.Send(new RefreshTokenCommand(dto));

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
        .WithName("RefreshToken");
    }
}