using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Core.Mapper;
using StudyFlow.Core.Queries.AiRequest.Response;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;
using StudyFlow.Core.Helper;

namespace StudyFlow.Core.Queries.AiRequest
{
    public class GetAiRequestsQueryHandler : IRequestHandler<GetAiRequestsQuery, Result<List<GetAiRequestDto>>>
    {
        private readonly StudyFlowDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public GetAiRequestsQueryHandler(StudyFlowDbContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }

        public async Task<Result<List<GetAiRequestDto>>> Handle(GetAiRequestsQuery request, CancellationToken cancellationToken)
        {
            int userId = _currentUserService.GetUserId();

            var aiRequests = await _dbContext.AiRequests
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .ToListAsync(cancellationToken);

            if (aiRequests.Count == 0)
            {
                return Result<List<GetAiRequestDto>>.Failure(AiRequestErrors.AiRequestNotFound);
            }

            return Result<List<GetAiRequestDto>>.Success(aiRequests.Select(x => x.ToDto()).ToList());
        }
    }
}
