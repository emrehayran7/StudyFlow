using System;

namespace StudyFlow.Core.Queries.Note.Response
{
    public class GetNoteDto
    {
        public int Id { get; set; }
        public int TopicId { get; set; }
        public string Title { get; set; } = null!;
        public string? Content { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}