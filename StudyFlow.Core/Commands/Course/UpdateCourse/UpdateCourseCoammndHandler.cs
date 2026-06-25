using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Core.Commands.Course.UpdateCourse.Request;
using StudyFlow.Core.Mapper;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;
using CourseEntity = StudyFlow.Domain.Entities.Course;
using StudyFlow.Core.Helper;

namespace StudyFlow.Core.Commands.Course.UpdateCourse
{
    public class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand, Result<int>>
    {
        private readonly StudyFlowDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public UpdateCourseCommandHandler(StudyFlowDbContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }

        public async Task<Result<int>> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
        {
            int userId = _currentUserService.GetUserId();

            if (string.IsNullOrWhiteSpace(request.updateCourseDto.Title))
            {
                return Result<int>.Failure(CourseErrors.TitleRequired);
            }

            CourseEntity course = await _dbContext.Courses
                .FirstOrDefaultAsync(
                    x => x.Id == request.CourseId &&
                         x.UserCourses.Any(userCourse => userCourse.UserId == userId),
                    cancellationToken);

            if (course == null)
            {
                return Result<int>.Failure(CourseErrors.CourseNotFound);
            }

            request.updateCourseDto.MapToEntity(course);

            course.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result<int>.Success(course.Id);
        }
    }
}
