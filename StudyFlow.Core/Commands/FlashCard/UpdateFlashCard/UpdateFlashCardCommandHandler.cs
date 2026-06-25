using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Core.Commands.FlashCard.UpdateFlashCard.Request;
using StudyFlow.Core.Mapper;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;
using FlashCardEntity = StudyFlow.Domain.Entities.FlashCard;
using StudyFlow.Core.Helper;

namespace StudyFlow.Core.Commands.FlashCard.UpdateFlashCard
{
    public class UpdateFlashCardCommandHandler : IRequestHandler<UpdateFlashCardCommand, Result<int>>
    {
        private readonly StudyFlowDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public UpdateFlashCardCommandHandler(StudyFlowDbContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }

        public async Task<Result<int>> Handle(UpdateFlashCardCommand request, CancellationToken cancellationToken)
        {
            int userId = _currentUserService.GetUserId();

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
                         x.Topic.Course.UserCourses.Any(userCourse => userCourse.UserId == userId),
                    cancellationToken);

            if (flashCard == null)
            {
                return Result<int>.Failure(FlashCardErrors.FlashCardNotFound);
            }

            dto.MapToEntity(flashCard);

            flashCard.UpdatedAt = DateTime.UtcNow;
            flashCard.UpdatedBy = userId.ToString();

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result<int>.Success(flashCard.Id);
        }
    }
}
