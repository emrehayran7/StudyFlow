using MediatR;
using StudyFlow.Core.Results;

namespace StudyFlow.Core.Commands.FlashCard.UpdateFlashCard.Request
{
    public record UpdateFlashCardCommand(int FlashCardId, UpdateFlashCardDto UpdateFlashCardDto, int UserId)
        : IRequest<Result<int>>;
}