namespace StudyFlow.Core.Commands.FlashCard.GenerateFlashCardsWithAi.Request
{
    public class GeneratedFlashCardCacheDto
    {
        public string TempId { get; set; } = null!;
        public int TopicId { get; set; }
        public string Question { get; set; } = null!;
        public string Answer { get; set; } = null!;
        public string Hint { get; set; } = null!;
        public int DifficultyLevel { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = null!;
    }
}
