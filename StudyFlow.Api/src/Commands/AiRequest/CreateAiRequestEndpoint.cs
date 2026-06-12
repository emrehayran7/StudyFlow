using MediatR;
using StudyFlow.Api.src.Extensions;
using StudyFlow.Core.Commands.AiRequest.CreateAiRequest.Request;
using StudyFlow.Core.Results;
using System.Security.Claims;

namespace StudyFlow.Api.src.Commands.AiRequest
{
    public static class CreateAiRequestEndpoint
    {
        public static void CreateAiRequest(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/airequests", async (CreateAiRequestDto dto, ClaimsPrincipal user, IMediator mediator) =>
            {
                int userId = user.GetUserId();
                var result = await mediator.Send(new CreateAiRequestCommand(dto, userId));

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
            .WithName("CreateAiRequest")
            .RequireAuthorization();
        }
    }
}
