using MediatR;
using StudyFlow.Core.Results;

namespace StudyFlow.Core.Queries.AiRequest.Response
{
    public record GetAiRequestsQuery(int UserId) : IRequest<Result<List<GetAiRequestDto>>>;
}
