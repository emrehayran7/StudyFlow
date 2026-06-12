using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StudyFlow.Core.Commands.Course.CreateCourse.Request
{
    public class CreateCourseDto
    {
        [Required(ErrorMessage = "Course title is required.")]
        [StringLength(255, ErrorMessage = "Title cannot exceed 255 characters.")]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }
    }
}
