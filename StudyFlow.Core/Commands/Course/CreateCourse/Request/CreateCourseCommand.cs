using MediatR;
using StudyFlow.Core.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudyFlow.Core.Commands.Course.CreateCourse.Request
{
    public record CreateCourseCommand(CreateCourseDto createCourseDto) : IRequest<Result<int>>;
    
}
