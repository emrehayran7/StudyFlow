using System.ComponentModel.DataAnnotations;

namespace StudyFlow.Core.Commands.StudySession.UpdateStudySession.Request
{
    public class UpdateStudySessionDto
    {
        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public int DurationMinutes { get; set; }

        public string? SessionNotes { get; set; }
    }
}