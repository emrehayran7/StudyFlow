using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudyFlow.Core.Queries.Course.Response
{
    public record GetCourseByIdQuery(int CourseId) : IRequest<GetCourseDto>;
    
}
