using MediatR;
using StudyFlow.Core.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudyFlow.Core.Commands.Topic.CreateTopic.Request
{
    public record CreateTopicCommand(CreateTopicDto createTopicDto, int UserId) : IRequest<Result<int>>;
}
