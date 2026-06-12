using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Core.Mapper;
using StudyFlow.Core.Queries.StudySession.Response;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;
using StudySessionEntity = StudyFlow.Domain.Entities.StudySession;

namespace StudyFlow.Core.Queries.StudySession
{
    public class GetStudySessionByIdQueryHandler : IRequestHandler<GetStudySessionByIdQuery, Result<GetStudySessionDto>>
    {
        private readonly StudyFlowDbContext _dbContext;

        public GetStudySessionByIdQueryHandler(StudyFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<GetStudySessionDto>> Handle(GetStudySessionByIdQuery request, CancellationToken cancellationToken)
        {
            StudySessionEntity session = await _dbContext.StudySessions
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.StudySessionId && x.UserId == request.UserId,
                    cancellationToken);

            if (session == null)
            {
                return Result<GetStudySessionDto>.Failure(StudySessionErrors.StudySessionNotFound);
            }

            return Result<GetStudySessionDto>.Success(session.ToDto());
        }
    }
}
