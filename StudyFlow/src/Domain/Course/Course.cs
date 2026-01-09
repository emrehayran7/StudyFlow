namespace StudyFlow.src.Domain.Course
{
    public class Course
    {
        
        public Guid Id { get; set; }

        public string Title { get;  set; }
        public string Description { get;  set; }

        public DateTime CreatedAt { get;  set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
