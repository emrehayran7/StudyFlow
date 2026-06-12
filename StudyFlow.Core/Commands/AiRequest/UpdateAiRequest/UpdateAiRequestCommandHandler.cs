using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Core.Commands.AiRequest.UpdateAiRequest.Request;
using StudyFlow.Core.Mapper;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;
using AiRequestEntity = StudyFlow.Domain.Entities.AiRequest;

namespace StudyFlow.Core.Commands.AiRequest.UpdateAiRequest
{
    public class UpdateAiRequestCommandHandler : IRequestHandler<UpdateAiRequestCommand, Result<int>>
    {
        private readonly StudyFlowDbContext _dbContext;

        public UpdateAiRequestCommandHandler(StudyFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<int>> Handle(UpdateAiRequestCommand request, CancellationToken cancellationToken)
        {
            var dto = request.UpdateAiRequestDto;

            if (string.IsNullOrWhiteSpace(dto.RequestType))
            {
                return Result<int>.Failure(AiRequestErrors.RequestTypeRequired);
            }

            if (string.IsNullOrWhiteSpace(dto.InputPrompt))
            {
                return Result<int>.Failure(AiRequestErrors.InputPromptRequired);
            }

            AiRequestEntity aiRequest = await _dbContext.AiRequests
                .FirstOrDefaultAsync(
                    x => x.Id == request.AiRequestId && x.UserId == request.UserId,
                    cancellationToken);

            if (aiRequest == null)
            {
                return Result<int>.Failure(AiRequestErrors.AiRequestNotFound);
            }

            dto.MapToEntity(aiRequest);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result<int>.Success(aiRequest.Id);
        }
    }
}
