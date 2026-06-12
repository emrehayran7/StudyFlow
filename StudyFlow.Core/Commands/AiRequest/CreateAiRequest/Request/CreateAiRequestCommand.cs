using MediatR;
using StudyFlow.Core.Results;

namespace StudyFlow.Core.Commands.AiRequest.CreateAiRequest.Request
{
    public record CreateAiRequestCommand(CreateAiRequestDto CreateAiRequestDto, int UserId) : IRequest<Result<int>>;
}
