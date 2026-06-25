using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Core.Commands.FlashCard.DeleteFlashCard.Request;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;
using FlashCardEntity = StudyFlow.Domain.Entities.FlashCard;
using StudyFlow.Core.Helper;

namespace StudyFlow.Core.Commands.FlashCard.DeleteFlashCard
{
    public class DeleteFlashCardCommandHandler : IRequestHandler<DeleteFlashCardCommand, Result<bool>>
    {
        private readonly StudyFlowDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public DeleteFlashCardCommandHandler(StudyFlowDbContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }

        public async Task<Result<bool>> Handle(DeleteFlashCardCommand request, CancellationToken cancellationToken)
        {
            int userId = _currentUserService.GetUserId();

            FlashCardEntity flashCard = await _dbContext.FlashCards
                .FirstOrDefaultAsync(
                    x => x.Id == request.FlashCardId &&
                         x.Topic.Course.UserCourses.Any(userCourse => userCourse.UserId == userId),
                    cancellationToken);

            if (flashCard == null)
            {
                return Result<bool>.Failure(FlashCardErrors.FlashCardNotFound);
            }

           

            _dbContext.FlashCards.Remove(flashCard);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
