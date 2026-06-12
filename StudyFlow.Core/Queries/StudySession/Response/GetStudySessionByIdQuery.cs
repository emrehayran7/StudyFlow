using MediatR;
using StudyFlow.Core.Queries.StudySession.Response;
using StudyFlow.Core.Results;

namespace StudyFlow.Core.Queries.StudySession
{
    public record GetStudySessionByIdQuery(int StudySessionId, int UserId)
        : IRequest<Result<GetStudySessionDto>>;
}
