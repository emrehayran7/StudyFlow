namespace StudyFlow.Core.Commands.FlashCard.GenerateFlashCardsWithAi.Request
{
    public class GenerateFlashCardsWithAiDto
    {
        public int TopicId { get; set; }

        public int FlashCardCount { get; set; } = 5;

        public int DifficultyLevel { get; set; } = 1;
    }
}
