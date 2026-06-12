namespace StudyFlow.Core.Commands.FlashCard.GenerateFlashCardsWithAi
{
    public class GeneratedFlashCard
    {
        public string Question { get; set; } = null!;
        public string Answer { get; set; } = null!;
        public string? Hint { get; set; }
        public int? DifficultyLevel { get; set; }
    }
}
