using AutoMapper;
using MediatR;
using StudyFlow.Core.Commands.Course.CreateCourse.Request;
using StudyFlow.Core.Mapper;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;

namespace StudyFlow.Core.Commands.Course.CreateCourse
{
    public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, Result<int>>
    {
        private readonly StudyFlowDbContext _dbContext;

        public CreateCourseCommandHandler(StudyFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<int>> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.createCourseDto.Title))
            {
                return Result<int>.Failure(CourseErrors.TitleRequired);
            }

            Domain.Entities.Course course = request.createCourseDto.ToEntity();
            course.CreatedAt = DateTime.UtcNow;

            UserCourse userCourse = request.ToUserCourse();
            course.UserCourses.Add(userCourse);

            _dbContext.Courses.Add(course);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result<int>.Success(course.Id);
        }
    }
}