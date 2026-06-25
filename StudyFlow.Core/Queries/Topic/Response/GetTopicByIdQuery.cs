using MediatR;
using StudyFlow.Core.Queries.Topic.Response.StudyFlow.Core.Queries.Topic.Response;
using StudyFlow.Core.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudyFlow.Core.Queries.Topic.Response
{
    public record GetTopicByIdQuery(int TopicId) : IRequest<Result<GetTopicDto>>;
}
