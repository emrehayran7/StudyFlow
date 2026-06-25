using MediatR;
using StudyFlow.Core.Queries.Topic.Response;
using StudyFlow.Core.Queries.Topic.Response.StudyFlow.Core.Queries.Topic.Response;
using StudyFlow.Core.Results;

namespace StudyFlow.Api.src.Queries.Topic
{
    public static class GetTopicByIdEndpoint
    {
        public static void GetTopicById(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/topics/{topicId:int}", async (int topicId, IMediator mediator) =>
            {
                GetTopicByIdQuery query = new GetTopicByIdQuery(topicId);

                Result<GetTopicDto> result = await mediator.Send(query);

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
            .WithName("GetTopicById")
            .RequireAuthorization();
        }
    }
}
