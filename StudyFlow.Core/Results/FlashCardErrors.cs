namespace StudyFlow.Core.Results
{
    public static class FlashCardErrors
    {
        public static readonly Error QuestionRequired =
            new Error("FlashCard.QuestionRequired", "Question cannot be empty.", ErrorType.Validation);

        public static readonly Error AnswerRequired =
            new Error("FlashCard.AnswerRequired", "Answer cannot be empty.", ErrorType.Validation);

        public static readonly Error TopicNotFound =
            new Error("FlashCard.TopicNotFound", "Topic not found.", ErrorType.NotFound);
        public static readonly Error FlashCardNotFound =
    new Error("FlashCard.FlashCardNotFound", "FlashCard not found.", ErrorType.NotFound);

        public static readonly Error AiPromptRequired =
            new Error("FlashCard.AiPromptRequired", "AI prompt cannot be null or empty.", ErrorType.Validation);

        public static readonly Error AiGenerationFailed =
            new Error("FlashCard.AiGenerationFailed", "AI flashcard generation failed.", ErrorType.Validation);

        public static readonly Error AiResponseInvalid =
            new Error("FlashCard.AiResponseInvalid", "AI response format is invalid.", ErrorType.Validation);

        public static readonly Error SelectedFlashCardsRequired =
            new Error("FlashCard.SelectedFlashCardsRequired", "At least one selected flashcard is required.", ErrorType.Validation);

        public static readonly Error GeneratedFlashCardsNotFound =
            new Error("FlashCard.GeneratedFlashCardsNotFound", "Generated flashcards were not found in cache.", ErrorType.NotFound);

        public static readonly Error SelectedFlashCardsNotFound =
            new Error("FlashCard.SelectedFlashCardsNotFound", "One or more selected flashcards were not found in cache.", ErrorType.NotFound);
    }
}
