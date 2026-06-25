using MediatR;
using StudyFlow.Core.Commands.Topic.UpdateTopic.Request;
using StudyFlow.Core.Results;

namespace StudyFlow.Api.src.Commands.Topic
{
    public static class UpdateTopicEndpoint
    {
        public static void UpdateTopic(this IEndpointRouteBuilder app)
        {
            app.MapPut("/api/topics/{topicId:int}", async (int topicId, UpdateTopicDto dto, IMediator mediator) =>
            {
                UpdateTopicCommand command = new UpdateTopicCommand(topicId, dto);

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
            .WithName("UpdateTopic")
            .RequireAuthorization();
        }
    }
}
