using MediatR;
using StudyFlow.Core.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudyFlow.Core.Commands.Topic.UpdateTopic.Request
{
    public record UpdateTopicCommand(int TopicId, UpdateTopicDto updateTopicDto, int UserId) : IRequest<Result<int>>;
}
