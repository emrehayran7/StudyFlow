using MediatR;
using StudyFlow.Core.Commands.Auth.Response;
using StudyFlow.Core.Results;

namespace StudyFlow.Core.Commands.Auth.Register.Request
{
    public record RegisterCommand(RegisterDto RegisterDto) : IRequest<Result<AuthResponseDto>>;
}
