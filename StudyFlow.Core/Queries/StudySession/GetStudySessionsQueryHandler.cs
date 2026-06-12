using MediatR;
using StudyFlow.Core.Mapper;
using StudyFlow.Core.Queries.StudySession.Response;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Repository;

namespace StudyFlow.Core.Queries.StudySession
{
    public class GetStudySessionsQueryHandler : IRequestHandler<GetStudySessionsQuery, Result<List<GetStudySessionDto>>>
    {
        private readonly IStudyFlowRepository _repository;

        public GetStudySessionsQueryHandler(IStudyFlowRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<List<GetStudySessionDto>>> Handle(GetStudySessionsQuery request, CancellationToken cancellationToken)
        {
            var sessions = await _repository.GetStudySessionsByUserIdAsync(request.UserId, cancellationToken);

            if (sessions.Count == 0)
            {
                return Result<List<GetStudySessionDto>>.Failure(StudySessionErrors.StudySessionNotFound);
            }

            return Result<List<GetStudySessionDto>>.Success(sessions.Select(x => x.ToDto()).ToList());
        }
    }
}
