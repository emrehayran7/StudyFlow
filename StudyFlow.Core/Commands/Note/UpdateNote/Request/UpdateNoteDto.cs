using System.ComponentModel.DataAnnotations;

namespace StudyFlow.Core.Commands.Note.UpdateNote.Request
{
    public class UpdateNoteDto
    {
        [Required(ErrorMessage = "Note title is required.")]
        [StringLength(255, ErrorMessage = "Title cannot exceed 255 characters.")]
        public string Title { get; set; } = null!;

        public string? Content { get; set; }
    }
}