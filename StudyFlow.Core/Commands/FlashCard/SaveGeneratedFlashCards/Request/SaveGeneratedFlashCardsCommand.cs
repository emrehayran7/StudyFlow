using MediatR;
using StudyFlow.Core.Results;

namespace StudyFlow.Core.Commands.FlashCard.SaveGeneratedFlashCards.Request
{
    public record SaveGeneratedFlashCardsCommand(SaveGeneratedFlashCardsDto SaveGeneratedFlashCardsDto, int UserId)
        : IRequest<Result<List<int>>>;
}
