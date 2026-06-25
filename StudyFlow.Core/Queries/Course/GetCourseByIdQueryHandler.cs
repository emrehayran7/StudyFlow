using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Core.Queries.Course.Response;
using StudyFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using StudyFlow.Core.Helper;

namespace StudyFlow.Core.Queries.Course
{
    public class GetCourseByIdQueryHandler : IRequestHandler<GetCourseByIdQuery, GetCourseDto>
    {
        private StudyFlowDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public GetCourseByIdQueryHandler(StudyFlowDbContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }
        public async Task<GetCourseDto> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
        {
            int userId = _currentUserService.GetUserId();

            var userCourse = await _dbContext.UserCourses
                .Include(uc => uc.Course)
                .FirstOrDefaultAsync(
                    uc => uc.UserId == userId && uc.CourseId == request.CourseId,
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