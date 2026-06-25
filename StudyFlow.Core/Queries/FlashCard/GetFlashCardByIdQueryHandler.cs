using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Core.Mapper;
using StudyFlow.Core.Queries.FlashCard.Response;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;
using FlashCardEntity = StudyFlow.Domain.Entities.FlashCard;
using StudyFlow.Core.Helper;

namespace StudyFlow.Core.Queries.FlashCard
{
    public class GetFlashCardByIdQueryHandler : IRequestHandler<GetFlashCardByIdQuery, Result<GetFlashCardDto>>
    {
        private readonly StudyFlowDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public GetFlashCardByIdQueryHandler(StudyFlowDbContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }

        public async Task<Result<GetFlashCardDto>> Handle(GetFlashCardByIdQuery request, CancellationToken cancellationToken)
        {
            int userId = _currentUserService.GetUserId();

            FlashCardEntity flashCard = await _dbContext.FlashCards
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.FlashCardId &&
                         x.Topic.Course.UserCourses.Any(userCourse => userCourse.UserId == userId),
                    cancellationToken);

            if (flashCard == null)
            {
                return Result<GetFlashCardDto>.Failure(FlashCardErrors.FlashCardNotFound);
            }

            return Result<GetFlashCardDto>.Success(flashCard.ToDto());
        }
    }
}
