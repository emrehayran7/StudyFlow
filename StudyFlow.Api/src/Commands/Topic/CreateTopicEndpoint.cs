using MediatR;
using StudyFlow.Api.src.Extensions;
using StudyFlow.Core.Commands.Topic.CreateTopic.Request;
using StudyFlow.Core.Results;
using System.Security.Claims;

namespace StudyFlow.Api.src.Commands.Topic
{
    public static class CreateTopicEndpoint
    {
        public static void CreateTopic(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/createtopic", async (CreateTopicDto dto, ClaimsPrincipal user, IMediator mediator) =>
            {
                int userId = user.GetUserId();
                CreateTopicCommand command = new CreateTopicCommand(dto, userId);

                Result<int> result = await mediator.Send(command);

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
            .WithName("CreateTopic")
            .RequireAuthorization();
        }
    }
}
