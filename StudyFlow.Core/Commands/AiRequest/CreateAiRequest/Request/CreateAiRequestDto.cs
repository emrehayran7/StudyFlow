using System.ComponentModel.DataAnnotations;

namespace StudyFlow.Core.Commands.AiRequest.CreateAiRequest.Request
{
    public class CreateAiRequestDto
    {
        [Required]
        public int TopicId { get; set; }

        [Required]
        public string RequestType { get; set; } = null!;

        [Required]
        public string InputPrompt { get; set; } = null!;

        public string? AiResponse { get; set; }
    }
}
