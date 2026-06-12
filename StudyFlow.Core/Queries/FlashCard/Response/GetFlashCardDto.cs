using System;

namespace StudyFlow.Core.Queries.FlashCard.Response
{
    public class GetFlashCardDto
    {
        public int Id { get; set; }
        public int TopicId { get; set; }
        public string Question { get; set; } = null!;
        public string Answer { get; set; } = null!;
        public string? Hint { get; set; }
        public int DifficultyLevel { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}