using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Core.Mapper;
using StudyFlow.Core.Queries.AiRequest.Response;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;

namespace StudyFlow.Core.Queries.AiRequest
{
    public class GetAiRequestsQueryHandler : IRequestHandler<GetAiRequestsQuery, Result<List<GetAiRequestDto>>>
    {
        private readonly StudyFlowDbContext _dbContext;

        public GetAiRequestsQueryHandler(StudyFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<List<GetAiRequestDto>>> Handle(GetAiRequestsQuery request, CancellationToken cancellationToken)
        {
            var aiRequests = await _dbContext.AiRequests
                .AsNoTracking()
                .Where(x => x.UserId == request.UserId)
                .ToListAsync(cancellationToken);

            if (aiRequests.Count == 0)
            {
                return Result<List<GetAiRequestDto>>.Failure(AiRequestErrors.AiRequestNotFound);
            }

            return Result<List<GetAiRequestDto>>.Success(aiRequests.Select(x => x.ToDto()).ToList());
        }
    }
}
