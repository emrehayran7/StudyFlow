using System.ComponentModel.DataAnnotations;

namespace StudyFlow.Core.Commands.FlashCard.SaveGeneratedFlashCards.Request
{
    public class SaveGeneratedFlashCardsDto
    {
        [Required]
        public int TopicId { get; set; }

        [Required]
        public List<string> TempIds { get; set; } = new();
    }
}
