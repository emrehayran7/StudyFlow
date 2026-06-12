using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StudyFlow.Core.Commands.Course.UpdateCourse.Request
{
    public class UpdateCourseDto
    {
        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }
    }
}
