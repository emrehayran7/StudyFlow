using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Core.Mapper;
using StudyFlow.Core.Queries.AiRequest.Response;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;
using AiRequestEntity = StudyFlow.Domain.Entities.AiRequest;
using StudyFlow.Core.Helper;

namespace StudyFlow.Core.Queries.AiRequest
{
    public class GetAiRequestByIdQueryHandler : IRequestHandler<GetAiRequestByIdQuery, Result<GetAiRequestDto>>
    {
        private readonly StudyFlowDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public GetAiRequestByIdQueryHandler(StudyFlowDbContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }

        public async Task<Result<GetAiRequestDto>> Handle(GetAiRequestByIdQuery request, CancellationToken cancellationToken)
        {
            int userId = _currentUserService.GetUserId();

            AiRequestEntity aiRequest = await _dbContext.AiRequests
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.AiRequestId && x.UserId == userId,
                    cancellationToken);

            if (aiRequest == null)
            {
                return Result<GetAiRequestDto>.Failure(AiRequestErrors.AiRequestNotFound);
            }

            return Result<GetAiRequestDto>.Success(aiRequest.ToDto());
        }
    }
}
