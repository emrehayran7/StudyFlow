using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Core.Commands.StudySession.DeleteStudySession.Request;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;
using StudySessionEntity = StudyFlow.Domain.Entities.StudySession;
using StudyFlow.Core.Helper;

namespace StudyFlow.Core.Commands.StudySession.DeleteStudySession
{
    public class DeleteStudySessionCommandHandler : IRequestHandler<DeleteStudySessionCommand, Result<bool>>
    {
        private readonly StudyFlowDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public DeleteStudySessionCommandHandler(StudyFlowDbContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }

        public async Task<Result<bool>> Handle(DeleteStudySessionCommand request, CancellationToken cancellationToken)
        {
            int userId = _currentUserService.GetUserId();

            StudySessionEntity session = await _dbContext.StudySessions
                .FirstOrDefaultAsync(
                    x => x.Id == request.StudySessionId &&
                         x.UserId == userId,
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
