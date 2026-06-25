using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Core.Mapper;
using StudyFlow.Core.Queries.Topic.Response;
using StudyFlow.Core.Queries.Topic.Response.StudyFlow.Core.Queries.Topic.Response;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;
using StudyFlow.Core.Helper;

namespace StudyFlow.Core.Queries.Topic
{
    public class GetTopicsQueryHandler : IRequestHandler<GetTopicsQuery, Result<List<GetTopicDto>>>
    {
        private readonly StudyFlowDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public GetTopicsQueryHandler(StudyFlowDbContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }

        public async Task<Result<List<GetTopicDto>>> Handle(GetTopicsQuery request, CancellationToken cancellationToken)
        {
            int userId = _currentUserService.GetUserId();

            var topics = await _dbContext.Topics
                .AsNoTracking()
                .Where(x =>
                    x.CourseId == request.CourseId &&
                    x.Course.UserCourses.Any(userCourse => userCourse.UserId == userId))
                .ToListAsync(cancellationToken);

            if (topics.Count == 0)
            {
                return Result<List<GetTopicDto>>.Failure(TopicErrors.TopicNotFound);
            }

            return Result<List<GetTopicDto>>.Success(topics.Select(x => x.ToDto()).ToList());
        }
    }
}
