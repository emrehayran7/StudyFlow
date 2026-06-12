using MediatR;
using StudyFlow.Core.Results;

namespace StudyFlow.Core.Commands.AiRequest.UpdateAiRequest.Request
{
    public record UpdateAiRequestCommand(int AiRequestId, UpdateAiRequestDto UpdateAiRequestDto, int UserId) : IRequest<Result<int>>;
}
