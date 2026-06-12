using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Core.Commands.AiRequest.CreateAiRequest.Request;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;
using AiRequestEntity = StudyFlow.Domain.Entities.AiRequest;

namespace StudyFlow.Core.Commands.AiRequest.CreateAiRequest
{
    public class CreateAiRequestCommandHandler : IRequestHandler<CreateAiRequestCommand, Result<int>>
    {
        private readonly StudyFlowDbContext _dbContext;

        public CreateAiRequestCommandHandler(StudyFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<int>> Handle(CreateAiRequestCommand request, CancellationToken cancellationToken)
        {
            var dto = request.CreateAiRequestDto;

            if (string.IsNullOrWhiteSpace(dto.RequestType))
            {
                return Result<int>.Failure(AiRequestErrors.RequestTypeRequired);
            }

            if (string.IsNullOrWhiteSpace(dto.InputPrompt))
            {
                return Result<int>.Failure(AiRequestErrors.InputPromptRequired);
            }

            bool userExists = await _dbContext.Users.AnyAsync(x => x.Id == request.UserId, cancellationToken);
            if (!userExists)
            {
                return Result<int>.Failure(AiRequestErrors.UserNotFound);
            }

            bool topicExists = await _dbContext.Topics
                .AnyAsync(
                    x => x.Id == dto.TopicId &&
                         x.Course.UserCourses.Any(userCourse => userCourse.UserId == request.UserId),
                    cancellationToken);

            if (!topicExists)
            {
                return Result<int>.Failure(AiRequestErrors.TopicNotFound);
            }

            AiRequestEntity aiRequest = new AiRequestEntity
            {
                UserId = request.UserId,
                TopicId = dto.TopicId,
                RequestType = dto.RequestType,
                InputPrompt = dto.InputPrompt,
                AiResponse = dto.AiResponse,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.AiRequests.Add(aiRequest);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result<int>.Success(aiRequest.Id);
        }
    }
}
