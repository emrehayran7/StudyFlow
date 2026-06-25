using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using StudyFlow.Core.Commands.FlashCard.GenerateFlashCardsWithAi.Request;
using StudyFlow.Core.Helper;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;
using System.Text;
using System.Text.Json;
using AiRequestEntity = StudyFlow.Domain.Entities.AiRequest;
using NoteEntity = StudyFlow.Domain.Entities.Note;

namespace StudyFlow.Core.Commands.FlashCard.GenerateFlashCardsWithAi
{
    public class GenerateFlashCardsWithAiCommandHandler
        : IRequestHandler<GenerateFlashCardsWithAiCommand, Result<List<GeneratedFlashCardCacheDto>>>
    {
        private readonly StudyFlowDbContext _dbContext;
        private readonly IAiService _aiService;
        private readonly IMemoryCache _memoryCache;
        private readonly ICurrentUserService _currentUserService;

        public GenerateFlashCardsWithAiCommandHandler(StudyFlowDbContext dbContext, IAiService aiService, IMemoryCache memoryCache, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _aiService = aiService;
            _memoryCache = memoryCache;
            _currentUserService = currentUserService;
        }

        public async Task<Result<List<GeneratedFlashCardCacheDto>>> Handle(
            GenerateFlashCardsWithAiCommand request,
            CancellationToken cancellationToken)
        {
            int userId = _currentUserService.GetUserId();

            GenerateFlashCardsWithAiDto dto = request.GenerateFlashCardsWithAiDto;

            bool userExists = await _dbContext.Users
                .AnyAsync(x => x.Id == userId, cancellationToken);

            if (!userExists)
            {
                return Result<List<GeneratedFlashCardCacheDto>>.Failure(AiRequestErrors.UserNotFound);
            }

            var topic = await _dbContext.Topics
                .AsNoTracking()
                .Include(x => x.Notes)
                .FirstOrDefaultAsync(
                    x => x.Id == dto.TopicId &&
                         x.Course.UserCourses.Any(userCourse => userCourse.UserId == userId),
                    cancellationToken);

            if (topic == null)
            {
                return Result<List<GeneratedFlashCardCacheDto>>.Failure(FlashCardErrors.TopicNotFound);
            }

            int flashCardCount = dto.FlashCardCount <= 0 ? 5 : dto.FlashCardCount;
            int difficultyLevel = dto.DifficultyLevel <= 0 ? 1 : dto.DifficultyLevel;
            string aiPrompt = BuildPrompt(topic.Title, topic.Description, topic.Notes, flashCardCount, difficultyLevel);

            string aiResponse;
            try
            {
                aiResponse = await _aiService.AskAsync(aiPrompt, cancellationToken);
            }
            catch
            {
                return Result<List<GeneratedFlashCardCacheDto>>.Failure(FlashCardErrors.AiGenerationFailed);
            }

            List<GeneratedFlashCard>? generatedFlashCards = ParseFlashCards(aiResponse);

            if (generatedFlashCards == null || generatedFlashCards.Count == 0)
            {
                return Result<List<GeneratedFlashCardCacheDto>>.Failure(FlashCardErrors.AiResponseInvalid);
            }
            DateTime now = DateTime.UtcNow;
            List<GeneratedFlashCardCacheDto> flashCardDtos = generatedFlashCards
                .Where(x => !string.IsNullOrWhiteSpace(x.Question) && !string.IsNullOrWhiteSpace(x.Answer))
                .Select(x => new GeneratedFlashCardCacheDto
                {
                    TempId = Guid.NewGuid().ToString("N"),
                    TopicId = dto.TopicId,
                    Question = x.Question.Trim(),
                    Answer = x.Answer.Trim(),
                    Hint = string.IsNullOrWhiteSpace(x.Hint) ? "Review" : TrimToMaxLength(x.Hint.Trim(), 50),
                    DifficultyLevel = x.DifficultyLevel.GetValueOrDefault(difficultyLevel),
                    CreatedAt = now,
                    CreatedBy = userId.ToString()
                })
                .ToList();

            if (flashCardDtos.Count == 0)
            {
                return Result<List<GeneratedFlashCardCacheDto>>.Failure(FlashCardErrors.AiResponseInvalid);
            }

            AiRequestEntity aiRequest = new AiRequestEntity
            {
                UserId = userId,
                TopicId = dto.TopicId,
                RequestType = "GenerateFlashCards",
                InputPrompt = aiPrompt,
                AiResponse = aiResponse,
                CreatedAt = now
            };

            string aiRequestCacheKey = GeneratedFlashCardCacheKeys.GetAiRequestKey(userId, dto.TopicId);
            string flashCardsCacheKey = GeneratedFlashCardCacheKeys.GetFlashCardsKey(userId, dto.TopicId);

            _memoryCache.Set(aiRequestCacheKey, aiRequest);
            _memoryCache.Set(flashCardsCacheKey, flashCardDtos);

            return Result<List<GeneratedFlashCardCacheDto>>.Success(flashCardDtos);
        }

        private static string BuildPrompt(
            string topicTitle,
            string? topicDescription,
            IEnumerable<NoteEntity> notes,
            int flashCardCount,
            int difficultyLevel)
        {
            string topicDescriptionText = string.IsNullOrWhiteSpace(topicDescription)
                ? string.Empty
                : $"Topic description: {topicDescription}{Environment.NewLine}";

            var promptBuilder = new StringBuilder($$"""
                    You are an expert study assistant.
                    Generate {{flashCardCount}} study flashcards in English.
                    Topic title: {{topicTitle}}
                    {{topicDescriptionText}}
                    Use the topic and the notes below as the only study context.
                    If notes are available, prioritize note content over general knowledge.
                    Make every question, answer, and hint English.
                    Use difficulty level {{difficultyLevel}} unless a card clearly needs another level.
                    Return only valid JSON. Do not include markdown, explanations, or code fences.
                    JSON schema: [{"question":"English question","answer":"English answer","hint":"short English hint","difficultyLevel":1}]
                    Notes:
                    """);

            var noteList = notes.ToList();
            if (noteList.Count == 0)
            {
                promptBuilder.AppendLine("- No notes were found for this topic. Generate flashcards from the topic title and description only.");
            }
            else
            {
                foreach (var note in noteList)
                {
                    promptBuilder.AppendLine($"- Title: {note.Title}");
                    promptBuilder.AppendLine($"  Content: {note.Content}");
                }
            }

            return promptBuilder.ToString();
        }

        private static List<GeneratedFlashCard>? ParseFlashCards(string aiResponse)
        {
            string json = aiResponse.Trim();

            if (json.StartsWith("```"))
            {
                int firstNewLineIndex = json.IndexOf('\n');
                int lastFenceIndex = json.LastIndexOf("```", StringComparison.Ordinal);

                if (firstNewLineIndex >= 0 && lastFenceIndex > firstNewLineIndex)
                {
                    json = json[(firstNewLineIndex + 1)..lastFenceIndex].Trim();
                }
            }

            try
            {
                return JsonSerializer.Deserialize<List<GeneratedFlashCard>>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
            catch
            {
                return null;
            }
        }

        private static string TrimToMaxLength(string value, int maxLength)
        {
            return value.Length <= maxLength ? value : value[..maxLength];
        }
    }
}
