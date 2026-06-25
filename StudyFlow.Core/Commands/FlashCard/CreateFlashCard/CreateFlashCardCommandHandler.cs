using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Core.Commands.FlashCard.CreateFlashCard.Request;
using StudyFlow.Core.Mapper;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;
using System.Security.Claims;
using FlashCardEntity = StudyFlow.Domain.Entities.FlashCard;
using StudyFlow.Core.Helper;

namespace StudyFlow.Core.Commands.FlashCard.CreateFlashCard
{
    public class CreateFlashCardCommandHandler : IRequestHandler<CreateFlashCardCommand, Result<int>>
    {
        private readonly StudyFlowDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public CreateFlashCardCommandHandler(StudyFlowDbContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }

        

        public async Task<Result<int>> Handle(CreateFlashCardCommand request, CancellationToken cancellationToken)
        {
            int userId = _currentUserService.GetUserId();

            var dto = request.CreateFlashCardDto;

            if (string.IsNullOrWhiteSpace(dto.Question))
            {
                return Result<int>.Failure(FlashCardErrors.QuestionRequired);
            }

            if (string.IsNullOrWhiteSpace(dto.Answer))
            {
                return Result<int>.Failure(FlashCardErrors.AnswerRequired);
            }

            bool topicExists = await _dbContext.Topics
                .AnyAsync(
                    x => x.Id == dto.TopicId &&
                         x.Course.UserCourses.Any(userCourse => userCourse.UserId == userId),
                    cancellationToken);

            if (!topicExists)
            {
                return Result<int>.Failure(FlashCardErrors.TopicNotFound);
            }

            FlashCardEntity flashCard = dto.ToEntity();

            flashCard.CreatedAt = DateTime.UtcNow;
            flashCard.CreatedBy = userId.ToString();

            _dbContext.FlashCards.Add(flashCard);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result<int>.Success(flashCard.Id);
        }
    }
}
