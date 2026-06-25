using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using StudyFlow.Core.Mapper;
using StudyFlow.Core.Queries.Course.Response;
using StudyFlow.Domain.Repository;
using StudyFlow.Core.Helper;

namespace StudyFlow.Core.Queries.Course
{
    public class GetCourseQueryHandler : IRequestHandler<GetCoursesQuery, List<GetCourseDto>>
    {
        private readonly IStudyFlowRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public GetCourseQueryHandler(IStudyFlowRepository repository, ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }
        public async Task<List<GetCourseDto>> Handle(GetCoursesQuery request, CancellationToken cancellationToken)
        {
            int userId = _currentUserService.GetUserId();

            var courses = await _repository.GetCoursesByUserIdAsync(userId, cancellationToken);

            if (courses == null || courses.Count == 0)
            {
                throw new KeyNotFoundException("No courses found for the user.");
            }

            return courses.Select(x => x.ToDto()).ToList();

        }
    }
}
