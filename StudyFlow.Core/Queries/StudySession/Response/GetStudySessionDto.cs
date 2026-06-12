using System;

namespace StudyFlow.Core.Queries.StudySession.Response
{
    public class GetStudySessionDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TopicId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int? DurationMinutes { get; set; }
        public string? SessionNotes { get; set; }
    }
}