using MediatR;
using StudyFlow.Core.Results;

namespace StudyFlow.Core.Commands.FlashCard.CreateFlashCard.Request
{
    public record CreateFlashCardCommand(CreateFlashCardDto CreateFlashCardDto, int UserId)
        : IRequest<Result<int>>;
}