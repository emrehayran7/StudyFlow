using MediatR;
using StudyFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Core.Commands.Course.DeleteCourse.Request;
using CourseEntity = StudyFlow.Domain.Entities.Course;
using StudyFlow.Core.Helper;
namespace StudyFlow.Core.Commands.Course.DeleteCourse
{
    public class DeleteCourseCommandHandler : IRequestHandler<DeleteCourseCommand, int>
    {
        private readonly StudyFlowDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public DeleteCourseCommandHandler(StudyFlowDbContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }

        public async Task<int> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
        {
            int userId = _currentUserService.GetUserId();

            CourseEntity course = await _dbContext.Courses
                .FirstOrDefaultAsync(
                    x => x.Id == request.CourseId &&
                         x.UserCourses.Any(userCourse => userCourse.UserId == userId),
                    cancellationToken);

            if (course == null)
            {
                throw new Exception("Course not found.");
            }

            List<UserCourse> userCourses = await _dbContext.UserCourses
                .Where(x => x.CourseId == request.CourseId)
                .ToListAsync(cancellationToken);

            if (userCourses.Any())
            {
                _dbContext.UserCourses.RemoveRange(userCourses);
            }

            _dbContext.Courses.Remove(course);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return course.Id;
        }
    }
}
