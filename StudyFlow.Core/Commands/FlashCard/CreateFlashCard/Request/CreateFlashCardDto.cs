using System.ComponentModel.DataAnnotations;

namespace StudyFlow.Core.Commands.FlashCard.CreateFlashCard.Request
{
    public class CreateFlashCardDto
    {
        [Required]
        public int TopicId { get; set; }

        [Required]
        public string Question { get; set; } = null!;

        [Required]
        public string Answer { get; set; } = null!;

        [StringLength(50)]
        public string? Hint { get; set; }

        public int DifficultyLevel { get; set; }
    }
}