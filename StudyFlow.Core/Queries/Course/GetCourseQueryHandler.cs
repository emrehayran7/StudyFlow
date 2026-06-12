using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using StudyFlow.Core.Mapper;
using StudyFlow.Core.Queries.Course.Response;
using StudyFlow.Domain.Repository;

namespace StudyFlow.Core.Queries.Course
{
    public class GetCourseQueryHandler : IRequestHandler<GetCoursesQuery, List<GetCourseDto>>
    {
        private readonly IStudyFlowRepository _repository;

        public GetCourseQueryHandler(IStudyFlowRepository repository)
        {
            _repository = repository;
        }
        public async Task<List<GetCourseDto>> Handle(GetCoursesQuery request, CancellationToken cancellationToken)
        {
            var courses = await _repository.GetCoursesByUserIdAsync(request.UserId, cancellationToken);

            if (courses == null || courses.Count == 0)
            {
                throw new KeyNotFoundException("No courses found for the user.");
            }

            return courses.Select(x => x.ToDto()).ToList();

        }
    }
}
