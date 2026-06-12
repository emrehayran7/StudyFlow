using MediatR;
using StudyFlow.Core.Results;

namespace StudyFlow.Core.Commands.FlashCard.DeleteFlashCard.Request
{
    public record DeleteFlashCardCommand(int FlashCardId, int UserId)
        : IRequest<Result<bool>>;
}