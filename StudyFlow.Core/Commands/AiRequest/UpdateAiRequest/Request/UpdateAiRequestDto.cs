using System.ComponentModel.DataAnnotations;

namespace StudyFlow.Core.Commands.AiRequest.UpdateAiRequest.Request
{
    public class UpdateAiRequestDto
    {
        [Required]
        public string RequestType { get; set; } = null!;

        [Required]
        public string InputPrompt { get; set; } = null!;

        public string? AiResponse { get; set; }
    }
}
