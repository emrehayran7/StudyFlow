using MediatR;
using StudyFlow.Core.Results;

namespace StudyFlow.Core.Queries.AiRequest.Response
{
    public record GetAiRequestByIdQuery(int AiRequestId, int UserId) : IRequest<Result<GetAiRequestDto>>;
}
