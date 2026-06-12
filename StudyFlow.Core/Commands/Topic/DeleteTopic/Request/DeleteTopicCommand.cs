using MediatR;
using StudyFlow.Core.Results;

namespace StudyFlow.Core.Commands.Topic.DeleteTopic.Request
{
    public record DeleteTopicCommand(int TopicId, int UserId) : IRequest<Result<bool>>;
}