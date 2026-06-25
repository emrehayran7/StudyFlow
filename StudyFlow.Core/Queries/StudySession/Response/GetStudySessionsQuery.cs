using MediatR;
using StudyFlow.Core.Results;

namespace StudyFlow.Core.Queries.StudySession.Response
{
    public record GetStudySessionsQuery() : IRequest<Result<List<GetStudySessionDto>>>;
}
