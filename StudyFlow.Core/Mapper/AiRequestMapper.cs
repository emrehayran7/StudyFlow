using StudyFlow.Core.Commands.AiRequest.CreateAiRequest.Request;
using StudyFlow.Core.Commands.AiRequest.UpdateAiRequest.Request;
using StudyFlow.Core.Queries.AiRequest.Response;
using StudyFlow.Domain.Entities;

namespace StudyFlow.Core.Mapper
{
    public static class AiRequestMapper
    {
        public static AiRequest ToEntity(this CreateAiRequestDto dto)
        {
            return new AiRequest
            {
                TopicId = dto.TopicId,
                RequestType = dto.RequestType,
                InputPrompt = dto.InputPrompt,
                AiResponse = dto.AiResponse
            };
        }

        public static void MapToEntity(this UpdateAiRequestDto dto, AiRequest aiRequest)
        {
            aiRequest.RequestType = dto.RequestType;
            aiRequest.InputPrompt = dto.InputPrompt;
            aiRequest.AiResponse = dto.AiResponse;
        }

        public static GetAiRequestDto ToDto(this AiRequest aiRequest)
        {
            return new GetAiRequestDto
            {
                Id = aiRequest.Id,
                UserId = aiRequest.UserId,
                TopicId = aiRequest.TopicId,
                RequestType = aiRequest.RequestType,
                InputPrompt = aiRequest.InputPrompt,
                AiResponse = aiRequest.AiResponse,
                CreatedAt = aiRequest.CreatedAt
            };
        }
    }
}
