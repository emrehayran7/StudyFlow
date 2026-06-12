using MediatR;
using StudyFlow.Core.Results;

namespace StudyFlow.Core.Commands.StudySession.DeleteStudySession.Request
{
    public record DeleteStudySessionCommand(int StudySessionId, int UserId)
        : IRequest<Result<bool>>;
}