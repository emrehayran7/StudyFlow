using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Core.Commands.AiRequest.DeleteAiRequest.Request;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;
using AiRequestEntity = StudyFlow.Domain.Entities.AiRequest;
using StudyFlow.Core.Helper;

namespace StudyFlow.Core.Commands.AiRequest.DeleteAiRequest
{
    public class DeleteAiRequestCommandHandler : IRequestHandler<DeleteAiRequestCommand, Result<bool>>
    {
        private readonly StudyFlowDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public DeleteAiRequestCommandHandler(StudyFlowDbContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }

        public async Task<Result<bool>> Handle(DeleteAiRequestCommand request, CancellationToken cancellationToken)
        {
            int userId = _currentUserService.GetUserId();

            AiRequestEntity aiRequest = await _dbContext.AiRequests
                .FirstOrDefaultAsync(
                    x => x.Id == request.AiRequestId && x.UserId == userId,
                    cancellationToken);

            if (aiRequest == null)
            {
                return Result<bool>.Failure(AiRequestErrors.AiRequestNotFound);
            }

            _dbContext.AiRequests.Remove(aiRequest);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
