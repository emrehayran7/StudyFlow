using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Core.Commands.AiRequest.UpdateAiRequest.Request;
using StudyFlow.Core.Mapper;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;
using AiRequestEntity = StudyFlow.Domain.Entities.AiRequest;
using StudyFlow.Core.Helper;

namespace StudyFlow.Core.Commands.AiRequest.UpdateAiRequest
{
    public class UpdateAiRequestCommandHandler : IRequestHandler<UpdateAiRequestCommand, Result<int>>
    {
        private readonly StudyFlowDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public UpdateAiRequestCommandHandler(StudyFlowDbContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }

        public async Task<Result<int>> Handle(UpdateAiRequestCommand request, CancellationToken cancellationToken)
        {
            int userId = _currentUserService.GetUserId();

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
                    x => x.Id == request.AiRequestId && x.UserId == userId,
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
