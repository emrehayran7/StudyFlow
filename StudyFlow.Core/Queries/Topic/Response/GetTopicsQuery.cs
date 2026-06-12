using MediatR;
using StudyFlow.Core.Queries.Topic.Response.StudyFlow.Core.Queries.Topic.Response;
using StudyFlow.Core.Results;

namespace StudyFlow.Core.Queries.Topic.Response
{
    public record GetTopicsQuery(int CourseId, int UserId) : IRequest<Result<List<GetTopicDto>>>;
}
