using MediatR;
using StudyFlow.Core.Queries.FlashCard.Response;
using StudyFlow.Core.Results;

namespace StudyFlow.Core.Queries.FlashCard
{
    public record GetFlashCardByIdQuery(int FlashCardId) : IRequest<Result<GetFlashCardDto>>;
}
