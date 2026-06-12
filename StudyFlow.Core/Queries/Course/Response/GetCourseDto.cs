using System;
using System.Collections.Generic;
using System.Text;

namespace StudyFlow.Core.Queries.Course.Response
{
    public class GetCourseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
    }
}
