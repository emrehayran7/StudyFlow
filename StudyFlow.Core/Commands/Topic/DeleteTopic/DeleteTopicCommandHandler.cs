using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Core.Commands.Topic.DeleteTopic.Request;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;
using TopicEntity = StudyFlow.Domain.Entities.Topic;
using StudyFlow.Core.Helper;

namespace StudyFlow.Core.Commands.Topic.DeleteTopic
{
    public class DeleteTopicCommandHandler : IRequestHandler<DeleteTopicCommand, Result<bool>>
    {
        private readonly StudyFlowDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public DeleteTopicCommandHandler(StudyFlowDbContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }

        public async Task<Result<bool>> Handle(DeleteTopicCommand request, CancellationToken cancellationToken)
        {
            int userId = _currentUserService.GetUserId();

            TopicEntity topic = await _dbContext.Topics
                .FirstOrDefaultAsync(
                    x => x.Id == request.TopicId &&
                         x.Course.UserCourses.Any(userCourse => userCourse.UserId == userId),
                    cancellationToken);

            if (topic == null)
            {
                return Result<bool>.Failure(TopicErrors.TopicNotFound);
            }

            _dbContext.Topics.Remove(topic);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
