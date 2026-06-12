using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Core.Commands.FlashCard.CreateFlashCard.Request;
using StudyFlow.Core.Mapper;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;
using System.Security.Claims;
using FlashCardEntity = StudyFlow.Domain.Entities.FlashCard;

namespace StudyFlow.Core.Commands.FlashCard.CreateFlashCard
{
    public class CreateFlashCardCommandHandler : IRequestHandler<CreateFlashCardCommand, Result<int>>
    {
        private readonly StudyFlowDbContext _dbContext;

        public CreateFlashCardCommandHandler(StudyFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        

        public async Task<Result<int>> Handle(CreateFlashCardCommand request, CancellationToken cancellationToken)
        {
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
                         x.Course.UserCourses.Any(userCourse => userCourse.UserId == request.UserId),
                    cancellationToken);

            if (!topicExists)
            {
                return Result<int>.Failure(FlashCardErrors.TopicNotFound);
            }

            FlashCardEntity flashCard = dto.ToEntity();

            flashCard.CreatedAt = DateTime.UtcNow;
            flashCard.CreatedBy = request.UserId.ToString();

            _dbContext.FlashCards.Add(flashCard);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result<int>.Success(flashCard.Id);
        }
    }
}
