using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StudyFlow.Core.Commands.Topic.UpdateTopic.Request
{
    public class UpdateTopicDto
    {
        [Required]
        public int CourseId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = null!;

        public int? PriorityLevel { get; set; }
    }
}
