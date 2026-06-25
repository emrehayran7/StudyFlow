using MediatR;
using StudyFlow.Core.Commands.Topic.DeleteTopic.Request;
using StudyFlow.Core.Results;

namespace StudyFlow.Api.src.Commands.Topic
{
    public static class DeleteTopicEndpoint
    {
        public static void DeleteTopic(this IEndpointRouteBuilder app)
        {
            app.MapDelete("/api/topics/{topicId:int}", async (int topicId, IMediator mediator) =>
            {
                DeleteTopicCommand command = new DeleteTopicCommand(topicId);

                Result<bool> result = await mediator.Send(command);

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
            .WithName("DeleteTopic")
            .RequireAuthorization();
        }
    }
}
