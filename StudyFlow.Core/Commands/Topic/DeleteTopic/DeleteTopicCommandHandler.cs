using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Core.Commands.Topic.DeleteTopic.Request;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;
using TopicEntity = StudyFlow.Domain.Entities.Topic;

namespace StudyFlow.Core.Commands.Topic.DeleteTopic
{
    public class DeleteTopicCommandHandler : IRequestHandler<DeleteTopicCommand, Result<bool>>
    {
        private readonly StudyFlowDbContext _dbContext;

        public DeleteTopicCommandHandler(StudyFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<bool>> Handle(DeleteTopicCommand request, CancellationToken cancellationToken)
        {
            TopicEntity topic = await _dbContext.Topics
                .FirstOrDefaultAsync(
                    x => x.Id == request.TopicId &&
                         x.Course.UserCourses.Any(userCourse => userCourse.UserId == request.UserId),
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
