using System;
using System.Collections.Generic;
using System.Text;

namespace StudyFlow.Core.Results
{
    public static class CourseErrors
    {
        public static readonly Error TitleRequired =
            new Error(
                "400",
                "Course title cannot be null or empty.",
                ErrorType.Validation);

        public static readonly Error CourseNotFound =
            new Error(
                "404",
                "Course not found.",
                ErrorType.NotFound);

        public static readonly Error UserIdRequired =
            new Error(
                "400",
                "User id is required.",
                ErrorType.Validation);
    }
}
