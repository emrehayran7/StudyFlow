namespace StudyFlow.src.Domain.Course
{
    public class DummySession
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public int DurationMinutes { get; set; }
    }
}
