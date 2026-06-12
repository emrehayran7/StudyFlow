using System;

namespace StudyFlow.Core.Queries.AiRequest.Response
{
    public class GetAiRequestDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TopicId { get; set; }
        public string RequestType { get; set; } = null!;
        public string InputPrompt { get; set; } = null!;
        public string? AiResponse { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
