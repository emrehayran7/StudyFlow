using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Core.Mapper;
using StudyFlow.Core.Queries.FlashCard.Response;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;
using StudyFlow.Core.Helper;

namespace StudyFlow.Core.Queries.FlashCard
{
    public class GetFlashCardsQueryHandler : IRequestHandler<GetFlashCardsQuery, Result<List<GetFlashCardDto>>>
    {
        private readonly StudyFlowDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public GetFlashCardsQueryHandler(StudyFlowDbContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }

        public async Task<Result<List<GetFlashCardDto>>> Handle(GetFlashCardsQuery request, CancellationToken cancellationToken)
        {
            int userId = _currentUserService.GetUserId();

            var flashCards = await _dbContext.FlashCards
                .AsNoTracking()
                .Where(x =>
                    x.TopicId == request.TopicId &&
                    x.Topic.Course.UserCourses.Any(userCourse => userCourse.UserId == userId))
                .ToListAsync(cancellationToken);

            if (flashCards.Count == 0)
            {
                return Result<List<GetFlashCardDto>>.Failure(FlashCardErrors.FlashCardNotFound);
            }

            return Result<List<GetFlashCardDto>>.Success(flashCards.Select(x => x.ToDto()).ToList());
        }
    }
}
