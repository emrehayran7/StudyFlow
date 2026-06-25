using MediatR;
using StudyFlow.Core.Results;

namespace StudyFlow.Core.Commands.FlashCard.SaveGeneratedFlashCards.Request
{
    public record SaveGeneratedFlashCardsCommand(SaveGeneratedFlashCardsDto SaveGeneratedFlashCardsDto)
        : IRequest<Result<List<int>>>;
}
