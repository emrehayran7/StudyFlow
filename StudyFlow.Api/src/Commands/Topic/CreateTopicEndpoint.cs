using MediatR;
using StudyFlow.Core.Commands.Topic.CreateTopic.Request;
using StudyFlow.Core.Results;

namespace StudyFlow.Api.src.Commands.Topic
{
    public static class CreateTopicEndpoint
    {
        public static void CreateTopic(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/createtopic", async (CreateTopicDto dto, IMediator mediator) =>
            {
                CreateTopicCommand command = new CreateTopicCommand(dto);

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
