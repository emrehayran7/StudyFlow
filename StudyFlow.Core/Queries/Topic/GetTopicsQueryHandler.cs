using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Core.Mapper;
using StudyFlow.Core.Queries.Topic.Response;
using StudyFlow.Core.Queries.Topic.Response.StudyFlow.Core.Queries.Topic.Response;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;

namespace StudyFlow.Core.Queries.Topic
{
    public class GetTopicsQueryHandler : IRequestHandler<GetTopicsQuery, Result<List<GetTopicDto>>>
    {
        private readonly StudyFlowDbContext _dbContext;

        public GetTopicsQueryHandler(StudyFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<List<GetTopicDto>>> Handle(GetTopicsQuery request, CancellationToken cancellationToken)
        {
            var topics = await _dbContext.Topics
                .AsNoTracking()
                .Where(x =>
                    x.CourseId == request.CourseId &&
                    x.Course.UserCourses.Any(userCourse => userCourse.UserId == request.UserId))
                .ToListAsync(cancellationToken);

            if (topics.Count == 0)
            {
                return Result<List<GetTopicDto>>.Failure(TopicErrors.TopicNotFound);
            }

            return Result<List<GetTopicDto>>.Success(topics.Select(x => x.ToDto()).ToList());
        }
    }
}
