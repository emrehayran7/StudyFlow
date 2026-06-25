using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Core.Mapper;
using StudyFlow.Core.Queries.Topic.Response;
using StudyFlow.Core.Queries.Topic.Response.StudyFlow.Core.Queries.Topic.Response;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using StudyFlow.Core.Helper;

namespace StudyFlow.Core.Queries.Topic
{
    public class GetTopicByIdQueryHandler : IRequestHandler<GetTopicByIdQuery, Result<GetTopicDto>>
    {
        private readonly StudyFlowDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public GetTopicByIdQueryHandler(StudyFlowDbContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }

        public async Task<Result<GetTopicDto>> Handle(GetTopicByIdQuery request, CancellationToken cancellationToken)
        {
            int userId = _currentUserService.GetUserId();

            Domain.Entities.Topic topic = await _dbContext.Topics
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.TopicId &&
                         x.Course.UserCourses.Any(userCourse => userCourse.UserId == userId),
                    cancellationToken);

            if (topic == null)
            {
                return Result<GetTopicDto>.Failure(TopicErrors.TopicNotFound);
            }

            GetTopicDto dto = topic.ToDto();

            return Result<GetTopicDto>.Success(dto);
        }
    }
}
