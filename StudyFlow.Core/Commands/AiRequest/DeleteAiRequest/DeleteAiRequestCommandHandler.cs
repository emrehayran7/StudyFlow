using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Core.Commands.AiRequest.DeleteAiRequest.Request;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;
using AiRequestEntity = StudyFlow.Domain.Entities.AiRequest;

namespace StudyFlow.Core.Commands.AiRequest.DeleteAiRequest
{
    public class DeleteAiRequestCommandHandler : IRequestHandler<DeleteAiRequestCommand, Result<bool>>
    {
        private readonly StudyFlowDbContext _dbContext;

        public DeleteAiRequestCommandHandler(StudyFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<bool>> Handle(DeleteAiRequestCommand request, CancellationToken cancellationToken)
        {
            AiRequestEntity aiRequest = await _dbContext.AiRequests
                .FirstOrDefaultAsync(
                    x => x.Id == request.AiRequestId && x.UserId == request.UserId,
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
