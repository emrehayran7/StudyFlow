using System;
using System.Collections.Generic;
using System.Text;

namespace StudyFlow.Core.Queries.Topic.Response
{
    using System;

    namespace StudyFlow.Core.Queries.Topic.Response
    {
        public class GetTopicDto
        {
            public int Id { get; set; }
            public int CourseId { get; set; }
            public string Title { get; set; } = null!;
            public string? Description { get; set; }
            public string Status { get; set; } = null!;
            public int? PriorityLevel { get; set; }
            public DateTime? CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }
            public string? CreatedBy { get; set; }
            public string? UpdatedBy { get; set; }
        }
    }
}
