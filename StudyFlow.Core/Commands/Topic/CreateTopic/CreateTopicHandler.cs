using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Core.Commands.Topic.CreateTopic.Request;
using StudyFlow.Core.Mapper;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;
using StudyFlow.Core.Helper;

namespace StudyFlow.Core.Commands.Topic.CreateTopic
{
    public class CreateTopicCommandHandler : IRequestHandler<CreateTopicCommand, Result<int>>
    {
        private readonly StudyFlowDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public CreateTopicCommandHandler(StudyFlowDbContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }

        public async Task<Result<int>> Handle(CreateTopicCommand request, CancellationToken cancellationToken)
        {
            int userId = _currentUserService.GetUserId();

            if (string.IsNullOrWhiteSpace(request.createTopicDto.Title))
            {
                return Result<int>.Failure(TopicErrors.TitleRequired);
            }

            if (string.IsNullOrWhiteSpace(request.createTopicDto.Status))
            {
                return Result<int>.Failure(TopicErrors.StatusRequired);
            }

            bool courseExists = await _dbContext.Courses
                .AnyAsync(
                    x => x.Id == request.createTopicDto.CourseId &&
                         x.UserCourses.Any(userCourse => userCourse.UserId == userId),
                    cancellationToken);

            if (!courseExists)
            {
                return Result<int>.Failure(TopicErrors.CourseNotFound);
            }

            Domain.Entities.Topic topic = request.createTopicDto.ToEntity();
            topic.CreatedAt = DateTime.UtcNow;

            _dbContext.Topics.Add(topic);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result<int>.Success(topic.Id);
        }
    }
}
