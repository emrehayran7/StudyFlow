using MediatR;
using StudyFlow.Core.Results;

namespace StudyFlow.Core.Commands.AiRequest.CreateAiRequest.Request
{
    public record CreateAiRequestCommand(CreateAiRequestDto CreateAiRequestDto) : IRequest<Result<int>>;
}
