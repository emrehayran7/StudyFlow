using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Core.Commands.StudySession.UpdateStudySession.Request;
using StudyFlow.Core.Mapper;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;
using StudySessionEntity = StudyFlow.Domain.Entities.StudySession;

namespace StudyFlow.Core.Commands.StudySession.UpdateStudySession
{
    public class UpdateStudySessionCommandHandler : IRequestHandler<UpdateStudySessionCommand, Result<int>>
    {
        private readonly StudyFlowDbContext _dbContext;

        public UpdateStudySessionCommandHandler(StudyFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<int>> Handle(UpdateStudySessionCommand request, CancellationToken cancellationToken)
        {
            var dto = request.UpdateStudySessionDto;

            if (dto.DurationMinutes <= 0)
            {
                return Result<int>.Failure(StudySessionErrors.InvalidDuration);
            }

            StudySessionEntity session = await _dbContext.StudySessions
                .FirstOrDefaultAsync(
                    x => x.Id == request.StudySessionId &&
                         x.UserId == request.UserId,
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
