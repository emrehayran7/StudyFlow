using MediatR;
using StudyFlow.Core.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudyFlow.Core.Commands.Course.UpdateCourse.Request
{
    public record UpdateCourseCommand(int CourseId, UpdateCourseDto updateCourseDto, int UserId) : IRequest<Result<int>>;
}
