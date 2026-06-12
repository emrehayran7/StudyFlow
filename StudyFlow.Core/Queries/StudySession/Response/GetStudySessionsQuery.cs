using MediatR;
using StudyFlow.Core.Results;

namespace StudyFlow.Core.Queries.StudySession.Response
{
    public record GetStudySessionsQuery(int UserId) : IRequest<Result<List<GetStudySessionDto>>>;
}
