using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Core.Commands.Topic.UpdateTopic.Request;
using StudyFlow.Core.Mapper;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudyFlow.Core.Commands.Topic.UpdateTopic
{
    public class UpdateTopicHandler : IRequestHandler<UpdateTopicCommand, Result<int>>
    {
        private readonly StudyFlowDbContext _dbContext;

        public UpdateTopicHandler(StudyFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<int>> Handle(UpdateTopicCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.updateTopicDto.Title))
            {
                return Result<int>.Failure(TopicErrors.TitleRequired);
            }

            if (string.IsNullOrWhiteSpace(request.updateTopicDto.Status))
            {
                return Result<int>.Failure(TopicErrors.StatusRequired);
            }

            Domain.Entities.Topic topic = await _dbContext.Topics
                .FirstOrDefaultAsync(
                    x => x.Id == request.TopicId &&
                         x.Course.UserCourses.Any(userCourse => userCourse.UserId == request.UserId),
                    cancellationToken);

            if (topic == null)
            {
                return Result<int>.Failure(TopicErrors.TopicNotFound);
            }

            bool courseExists = await _dbContext.Courses
                .AnyAsync(
                    x => x.Id == request.updateTopicDto.CourseId &&
                         x.UserCourses.Any(userCourse => userCourse.UserId == request.UserId),
                    cancellationToken);

            if (!courseExists)
            {
                return Result<int>.Failure(TopicErrors.CourseNotFound);
            }

            request.updateTopicDto.MapToEntity(topic);
            topic.CourseId = request.updateTopicDto.CourseId;

            topic.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result<int>.Success(topic.Id);
        }
    }
}
