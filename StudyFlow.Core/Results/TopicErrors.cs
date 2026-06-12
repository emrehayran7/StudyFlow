using System;
using System.Collections.Generic;
using System.Text;

namespace StudyFlow.Core.Results
{
    public static class TopicErrors
    {
        public static readonly Error TitleRequired =
             new Error("Topic.TitleRequired", "Topic title cannot be null or empty.", ErrorType.Validation);

        public static readonly Error StatusRequired =
            new Error("Topic.StatusRequired", "Topic status cannot be null or empty.", ErrorType.Validation);

        public static readonly Error CourseNotFound =
            new Error("Topic.CourseNotFound", "Course not found.", ErrorType.NotFound);

        public static readonly Error TopicNotFound =
            new Error("Topic.TopicNotFound", "Topic not found.", ErrorType.NotFound);
    }
}
