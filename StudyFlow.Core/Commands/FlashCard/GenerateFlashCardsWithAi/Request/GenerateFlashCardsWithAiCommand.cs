using MediatR;
using StudyFlow.Core.Results;

namespace StudyFlow.Core.Commands.FlashCard.GenerateFlashCardsWithAi.Request
{
    public record GenerateFlashCardsWithAiCommand(GenerateFlashCardsWithAiDto GenerateFlashCardsWithAiDto)
        : IRequest<Result<List<GeneratedFlashCardCacheDto>>>;
}
