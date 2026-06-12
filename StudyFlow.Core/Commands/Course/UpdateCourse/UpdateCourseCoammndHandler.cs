using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Core.Commands.Course.UpdateCourse.Request;
using StudyFlow.Core.Mapper;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;
using CourseEntity = StudyFlow.Domain.Entities.Course;

namespace StudyFlow.Core.Commands.Course.UpdateCourse
{
    public class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand, Result<int>>
    {
        private readonly StudyFlowDbContext _dbContext;

        public UpdateCourseCommandHandler(StudyFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<int>> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.updateCourseDto.Title))
            {
                return Result<int>.Failure(CourseErrors.TitleRequired);
            }

            CourseEntity course = await _dbContext.Courses
                .FirstOrDefaultAsync(
                    x => x.Id == request.CourseId &&
                         x.UserCourses.Any(userCourse => userCourse.UserId == request.UserId),
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
