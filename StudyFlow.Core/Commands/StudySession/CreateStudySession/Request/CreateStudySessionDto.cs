using System.ComponentModel.DataAnnotations;

namespace StudyFlow.Core.Commands.StudySession.CreateStudySession.Request
{
    public class CreateStudySessionDto
    {
        [Required]
        public int TopicId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public int DurationMinutes { get; set; }

        public string? SessionNotes { get; set; }
    }
}