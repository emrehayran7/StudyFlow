using MediatR;
using StudyFlow.Core.Results;

namespace StudyFlow.Core.Commands.AiRequest.DeleteAiRequest.Request
{
    public record DeleteAiRequestCommand(int AiRequestId) : IRequest<Result<bool>>;
}
