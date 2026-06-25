using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using StudyFlow.Core.Commands.FlashCard.GenerateFlashCardsWithAi;
using StudyFlow.Core.Commands.FlashCard.GenerateFlashCardsWithAi.Request;
using StudyFlow.Core.Commands.FlashCard.SaveGeneratedFlashCards.Request;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;
using AiRequestEntity = StudyFlow.Domain.Entities.AiRequest;
using FlashCardEntity = StudyFlow.Domain.Entities.FlashCard;
using StudyFlow.Core.Helper;

namespace StudyFlow.Core.Commands.FlashCard.SaveGeneratedFlashCards
{
    public class SaveGeneratedFlashCardsCommandHandler : IRequestHandler<SaveGeneratedFlashCardsCommand, Result<List<int>>>
    {
        private readonly StudyFlowDbContext _dbContext;
        private readonly IMemoryCache _memoryCache;
        private readonly ICurrentUserService _currentUserService;

        public SaveGeneratedFlashCardsCommandHandler(StudyFlowDbContext dbContext, IMemoryCache memoryCache, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
            _currentUserService = currentUserService;
        }

        public async Task<Result<List<int>>> Handle(SaveGeneratedFlashCardsCommand request, CancellationToken cancellationToken)
        {
            int userId = _currentUserService.GetUserId();

            SaveGeneratedFlashCardsDto dto = request.SaveGeneratedFlashCardsDto;

            var selectedTempIds = dto.TempIds
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            if (selectedTempIds.Count == 0)
            {
                return Result<List<int>>.Failure(FlashCardErrors.SelectedFlashCardsRequired);
            }

            bool topicExists = await _dbContext.Topics
                .AnyAsync(
                    x => x.Id == dto.TopicId &&
                         x.Course.UserCourses.Any(userCourse => userCourse.UserId == userId),
                    cancellationToken);

            if (!topicExists)
            {
                return Result<List<int>>.Failure(FlashCardErrors.TopicNotFound);
            }

            string flashCardsCacheKey = GeneratedFlashCardCacheKeys.GetFlashCardsKey(userId, dto.TopicId);

            if (!_memoryCache.TryGetValue(flashCardsCacheKey, out List<GeneratedFlashCardCacheDto>? cachedFlashCards) ||
                cachedFlashCards == null ||
                cachedFlashCards.Count == 0)
            {
                return Result<List<int>>.Failure(FlashCardErrors.GeneratedFlashCardsNotFound);
            }

            List<GeneratedFlashCardCacheDto> selectedFlashCards = cachedFlashCards
                .Where(x => x.TopicId == dto.TopicId && selectedTempIds.Contains(x.TempId))
                .ToList();

            if (selectedFlashCards.Count != selectedTempIds.Count)
            {
                return Result<List<int>>.Failure(FlashCardErrors.SelectedFlashCardsNotFound);
            }

            List<FlashCardEntity> flashCards = selectedFlashCards
                .Select(x => new FlashCardEntity
                {
                    TopicId = x.TopicId,
                    Question = x.Question,
                    Answer = x.Answer,
                    Hint = x.Hint,
                    DifficultyLevel = x.DifficultyLevel,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = userId.ToString()
                })
                .ToList();

            _dbContext.FlashCards.AddRange(flashCards);

            string aiRequestCacheKey = GeneratedFlashCardCacheKeys.GetAiRequestKey(userId, dto.TopicId);

            if (_memoryCache.TryGetValue(aiRequestCacheKey, out AiRequestEntity? aiRequest) && aiRequest != null)
            {
                _dbContext.AiRequests.Add(aiRequest);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            _memoryCache.Remove(flashCardsCacheKey);
            _memoryCache.Remove(aiRequestCacheKey);

            return Result<List<int>>.Success(flashCards.Select(x => x.Id).ToList());
        }
    }
}
