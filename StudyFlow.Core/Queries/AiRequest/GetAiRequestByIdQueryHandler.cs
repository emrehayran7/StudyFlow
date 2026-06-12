using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Core.Mapper;
using StudyFlow.Core.Queries.AiRequest.Response;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;
using AiRequestEntity = StudyFlow.Domain.Entities.AiRequest;

namespace StudyFlow.Core.Queries.AiRequest
{
    public class GetAiRequestByIdQueryHandler : IRequestHandler<GetAiRequestByIdQuery, Result<GetAiRequestDto>>
    {
        private readonly StudyFlowDbContext _dbContext;

        public GetAiRequestByIdQueryHandler(StudyFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<GetAiRequestDto>> Handle(GetAiRequestByIdQuery request, CancellationToken cancellationToken)
        {
            AiRequestEntity aiRequest = await _dbContext.AiRequests
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.AiRequestId && x.UserId == request.UserId,
                    cancellationToken);

            if (aiRequest == null)
            {
                return Result<GetAiRequestDto>.Failure(AiRequestErrors.AiRequestNotFound);
            }

            return Result<GetAiRequestDto>.Success(aiRequest.ToDto());
        }
    }
}
