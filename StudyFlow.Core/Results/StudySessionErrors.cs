namespace StudyFlow.Core.Results
{
    public static class StudySessionErrors
    {
        public static readonly Error TopicNotFound =
            new Error("StudySession.TopicNotFound", "Topic not found.", ErrorType.NotFound);

        public static readonly Error InvalidTimeRange =
            new Error("StudySession.InvalidTimeRange", "End time must be later than start time.", ErrorType.Validation);
        public static readonly Error InvalidDuration =
    new Error("StudySession.InvalidDuration", "Duration must be greater than 0.", ErrorType.Validation);
        public static readonly Error StudySessionNotFound =
    new Error("StudySession.NotFound", "Study session not found.", ErrorType.NotFound);
    }
}