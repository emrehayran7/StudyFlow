using MediatR;
using StudyFlow.Core.Results;

namespace StudyFlow.Core.Queries.FlashCard.Response
{
    public record GetFlashCardsQuery(int TopicId, int UserId) : IRequest<Result<List<GetFlashCardDto>>>;
}
