using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Core.Commands.StudySession.DeleteStudySession.Request;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;
using StudySessionEntity = StudyFlow.Domain.Entities.StudySession;

namespace StudyFlow.Core.Commands.StudySession.DeleteStudySession
{
    public class DeleteStudySessionCommandHandler : IRequestHandler<DeleteStudySessionCommand, Result<bool>>
    {
        private readonly StudyFlowDbContext _dbContext;

        public DeleteStudySessionCommandHandler(StudyFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<bool>> Handle(DeleteStudySessionCommand request, CancellationToken cancellationToken)
        {
            StudySessionEntity session = await _dbContext.StudySessions
                .FirstOrDefaultAsync(
                    x => x.Id == request.StudySessionId &&
                         x.UserId == request.UserId,
                    cancellationToken);

            if (session == null)
            {
                return Result<bool>.Failure(StudySessionErrors.StudySessionNotFound);
            }

           

            _dbContext.StudySessions.Remove(session);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
