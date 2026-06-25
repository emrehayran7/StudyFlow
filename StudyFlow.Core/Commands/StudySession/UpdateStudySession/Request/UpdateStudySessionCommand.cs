using MediatR;
using StudyFlow.Core.Results;

namespace StudyFlow.Core.Commands.StudySession.UpdateStudySession.Request
{
    public record UpdateStudySessionCommand(int StudySessionId, UpdateStudySessionDto UpdateStudySessionDto)
        : IRequest<Result<int>>;
}