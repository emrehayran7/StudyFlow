using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Core.Commands.StudySession.UpdateStudySession.Request;
using StudyFlow.Core.Mapper;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;
using StudySessionEntity = StudyFlow.Domain.Entities.StudySession;
using StudyFlow.Core.Helper;

namespace StudyFlow.Core.Commands.StudySession.UpdateStudySession
{
    public class UpdateStudySessionCommandHandler : IRequestHandler<UpdateStudySessionCommand, Result<int>>
    {
        private readonly StudyFlowDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public UpdateStudySessionCommandHandler(StudyFlowDbContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }

        public async Task<Result<int>> Handle(UpdateStudySessionCommand request, CancellationToken cancellationToken)
        {
            int userId = _currentUserService.GetUserId();

            var dto = request.UpdateStudySessionDto;

            if (dto.DurationMinutes <= 0)
            {
                return Result<int>.Failure(StudySessionErrors.InvalidDuration);
            }

            StudySessionEntity session = await _dbContext.StudySessions
                .FirstOrDefaultAsync(
                    x => x.Id == request.StudySessionId &&
                         x.UserId == userId,
                    cancellationToken);

            if (session == null)
            {
                return Result<int>.Failure(StudySessionErrors.StudySessionNotFound);
            }

            dto.MapToEntity(session);

            session.EndTime = dto.StartTime.AddMinutes(dto.DurationMinutes);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result<int>.Success(session.Id);
        }
    }
}
