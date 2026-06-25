using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Core.Mapper;
using StudyFlow.Core.Queries.StudySession.Response;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;
using StudySessionEntity = StudyFlow.Domain.Entities.StudySession;
using StudyFlow.Core.Helper;

namespace StudyFlow.Core.Queries.StudySession
{
    public class GetStudySessionByIdQueryHandler : IRequestHandler<GetStudySessionByIdQuery, Result<GetStudySessionDto>>
    {
        private readonly StudyFlowDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public GetStudySessionByIdQueryHandler(StudyFlowDbContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }

        public async Task<Result<GetStudySessionDto>> Handle(GetStudySessionByIdQuery request, CancellationToken cancellationToken)
        {
            int userId = _currentUserService.GetUserId();

            StudySessionEntity session = await _dbContext.StudySessions
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.StudySessionId && x.UserId == userId,
                    cancellationToken);

            if (session == null)
            {
                return Result<GetStudySessionDto>.Failure(StudySessionErrors.StudySessionNotFound);
            }

            return Result<GetStudySessionDto>.Success(session.ToDto());
        }
    }
}
