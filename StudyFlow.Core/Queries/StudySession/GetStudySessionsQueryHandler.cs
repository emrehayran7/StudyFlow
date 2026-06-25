using MediatR;
using StudyFlow.Core.Mapper;
using StudyFlow.Core.Queries.StudySession.Response;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Repository;
using StudyFlow.Core.Helper;

namespace StudyFlow.Core.Queries.StudySession
{
    public class GetStudySessionsQueryHandler : IRequestHandler<GetStudySessionsQuery, Result<List<GetStudySessionDto>>>
    {
        private readonly IStudyFlowRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public GetStudySessionsQueryHandler(IStudyFlowRepository repository, ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<Result<List<GetStudySessionDto>>> Handle(GetStudySessionsQuery request, CancellationToken cancellationToken)
        {
            int userId = _currentUserService.GetUserId();

            var sessions = await _repository.GetStudySessionsByUserIdAsync(userId, cancellationToken);

            if (sessions.Count == 0)
            {
                return Result<List<GetStudySessionDto>>.Failure(StudySessionErrors.StudySessionNotFound);
            }

            return Result<List<GetStudySessionDto>>.Success(sessions.Select(x => x.ToDto()).ToList());
        }
    }
}
