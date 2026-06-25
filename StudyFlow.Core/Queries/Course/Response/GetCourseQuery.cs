using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
namespace StudyFlow.Core.Queries.Course.Response
{
    public record GetCoursesQuery() : IRequest<List<GetCourseDto>>;
}
