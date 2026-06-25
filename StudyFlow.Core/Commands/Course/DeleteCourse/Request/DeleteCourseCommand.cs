using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudyFlow.Core.Commands.Course.DeleteCourse.Request
{
    public record DeleteCourseCommand(int CourseId) : IRequest<int>;
}