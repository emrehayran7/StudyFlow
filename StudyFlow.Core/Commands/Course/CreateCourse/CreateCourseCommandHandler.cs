using MediatR;
using StudyFlow.Core.Commands.Course.CreateCourse.Request;
using StudyFlow.Core.Helper;
using StudyFlow.Core.Mapper;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;

namespace StudyFlow.Core.Commands.Course.CreateCourse
{
    public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, Result<int>>
    {
        private readonly StudyFlowDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public CreateCourseCommandHandler(StudyFlowDbContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }

        public async Task<Result<int>> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            int userId = _currentUserService.GetUserId();

            if (string.IsNullOrWhiteSpace(request.createCourseDto.Title))
            {
                return Result<int>.Failure(CourseErrors.TitleRequired);
            }

            Domain.Entities.Course course = request.createCourseDto.ToEntity();
            course.CreatedAt = DateTime.UtcNow;

            UserCourse userCourse = request.ToUserCourse(userId);
            course.UserCourses.Add(userCourse);

            _dbContext.Courses.Add(course);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result<int>.Success(course.Id);
        }
    }
}
