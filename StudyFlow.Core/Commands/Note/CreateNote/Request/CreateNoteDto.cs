using System.ComponentModel.DataAnnotations;

namespace StudyFlow.Core.Commands.Note.CreateNote.Request
{
    public class CreateNoteDto
    {
        [Required(ErrorMessage = "Topic id is required.")]
        public int TopicId { get; set; }

        [Required(ErrorMessage = "Note title is required.")]
        [StringLength(255, ErrorMessage = "Title cannot exceed 255 characters.")]
        public string Title { get; set; } = null!;

        public string? Content { get; set; }
    }
}