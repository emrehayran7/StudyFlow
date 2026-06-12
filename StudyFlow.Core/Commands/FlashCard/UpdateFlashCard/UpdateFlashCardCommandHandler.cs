using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Core.Commands.FlashCard.UpdateFlashCard.Request;
using StudyFlow.Core.Mapper;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;
using FlashCardEntity = StudyFlow.Domain.Entities.FlashCard;

namespace StudyFlow.Core.Commands.FlashCard.UpdateFlashCard
{
    public class UpdateFlashCardCommandHandler : IRequestHandler<UpdateFlashCardCommand, Result<int>>
    {
        private readonly StudyFlowDbContext _dbContext;

        public UpdateFlashCardCommandHandler(StudyFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<int>> Handle(UpdateFlashCardCommand request, CancellationToken cancellationToken)
        {
            var dto = request.UpdateFlashCardDto;

            if (string.IsNullOrWhiteSpace(dto.Question))
            {
                return Result<int>.Failure(FlashCardErrors.QuestionRequired);
            }

            if (string.IsNullOrWhiteSpace(dto.Answer))
            {
                return Result<int>.Failure(FlashCardErrors.AnswerRequired);
            }

            FlashCardEntity flashCard = await _dbContext.FlashCards
                .FirstOrDefaultAsync(
                    x => x.Id == request.FlashCardId &&
                         x.Topic.Course.UserCourses.Any(userCourse => userCourse.UserId == request.UserId),
                    cancellationToken);

            if (flashCard == null)
            {
                return Result<int>.Failure(FlashCardErrors.FlashCardNotFound);
            }

            dto.MapToEntity(flashCard);

            flashCard.UpdatedAt = DateTime.UtcNow;
            flashCard.UpdatedBy = request.UserId.ToString();

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result<int>.Success(flashCard.Id);
        }
    }
}
