using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Core.Queries.Course.Response;
using StudyFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudyFlow.Core.Queries.Course
{
    public class GetCourseByIdQueryHandler : IRequestHandler<GetCourseByIdQuery, GetCourseDto>
    {
        private StudyFlowDbContext _dbContext;

        public GetCourseByIdQueryHandler(StudyFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<GetCourseDto> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
        {
            var userCourse = await _dbContext.UserCourses
                .Include(uc => uc.Course)
                .FirstOrDefaultAsync(
                    uc => uc.UserId == request.UserId && uc.CourseId == request.CourseId,
                    cancellationToken);
            if (userCourse == null)
            {
                throw new KeyNotFoundException("Course not found for the user.");
            }
            return new GetCourseDto
            {
                Id = userCourse.Course.Id,
                Title = userCourse.Course.Title,
                Description = userCourse.Course.Description
            };
        }
    }
}