using MediatR;
using StudyFlow.Core.Results;

namespace StudyFlow.Core.Commands.StudySession.CreateStudySession.Request
{
    public record CreateStudySessionCommand(CreateStudySessionDto CreateStudySessionDto, int UserId)
        : IRequest<Result<int>>;
}