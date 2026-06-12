using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Core.Mapper;
using StudyFlow.Core.Queries.FlashCard.Response;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;
using FlashCardEntity = StudyFlow.Domain.Entities.FlashCard;

namespace StudyFlow.Core.Queries.FlashCard
{
    public class GetFlashCardByIdQueryHandler : IRequestHandler<GetFlashCardByIdQuery, Result<GetFlashCardDto>>
    {
        private readonly StudyFlowDbContext _dbContext;

        public GetFlashCardByIdQueryHandler(StudyFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<GetFlashCardDto>> Handle(GetFlashCardByIdQuery request, CancellationToken cancellationToken)
        {
            FlashCardEntity flashCard = await _dbContext.FlashCards
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.FlashCardId &&
                         x.Topic.Course.UserCourses.Any(userCourse => userCourse.UserId == request.UserId),
                    cancellationToken);

            if (flashCard == null)
            {
                return Result<GetFlashCardDto>.Failure(FlashCardErrors.FlashCardNotFound);
            }

            return Result<GetFlashCardDto>.Success(flashCard.ToDto());
        }
    }
}
