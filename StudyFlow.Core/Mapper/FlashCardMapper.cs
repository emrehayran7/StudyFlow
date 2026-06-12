using StudyFlow.Core.Commands.FlashCard.CreateFlashCard.Request;
using StudyFlow.Core.Commands.FlashCard.UpdateFlashCard.Request;
using StudyFlow.Core.Queries.FlashCard.Response;
using StudyFlow.Domain.Entities;

namespace StudyFlow.Core.Mapper
{
    public static class FlashCardMapper
    {
        public static FlashCard ToEntity(this CreateFlashCardDto dto)
        {
            return new FlashCard
            {
                TopicId = dto.TopicId,
                Question = dto.Question,
                Answer = dto.Answer,
                Hint = dto.Hint,
                DifficultyLevel = dto.DifficultyLevel
            };
        }

        public static void MapToEntity(this UpdateFlashCardDto dto, FlashCard entity)
        {
            entity.Question = dto.Question;
            entity.Answer = dto.Answer;
            entity.Hint = dto.Hint;
            entity.DifficultyLevel = dto.DifficultyLevel;
        }

        public static GetFlashCardDto ToDto(this FlashCard entity)
        {
            return new GetFlashCardDto
            {
                Id = entity.Id,
                TopicId = entity.TopicId,
                Question = entity.Question,
                Answer = entity.Answer,
                Hint = entity.Hint,
                DifficultyLevel = entity.DifficultyLevel,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                CreatedBy = entity.CreatedBy,
                UpdatedBy = entity.UpdatedBy
            };
        }
    }
}