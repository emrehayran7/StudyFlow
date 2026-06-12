namespace StudyFlow.Core.Results
{
    public static class AiRequestErrors
    {
        public static readonly Error RequestTypeRequired =
            new Error("AiRequest.RequestTypeRequired", "Request type cannot be null or empty.", ErrorType.Validation);

        public static readonly Error InputPromptRequired =
            new Error("AiRequest.InputPromptRequired", "Input prompt cannot be null or empty.", ErrorType.Validation);

        public static readonly Error TopicNotFound =
            new Error("AiRequest.TopicNotFound", "Topic not found.", ErrorType.NotFound);

        public static readonly Error UserNotFound =
            new Error("AiRequest.UserNotFound", "User not found.", ErrorType.NotFound);

        public static readonly Error AiRequestNotFound =
            new Error("AiRequest.NotFound", "AI request not found.", ErrorType.NotFound);
    }
}
