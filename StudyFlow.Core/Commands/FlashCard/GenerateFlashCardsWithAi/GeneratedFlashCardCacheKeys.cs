namespace StudyFlow.Core.Commands.FlashCard.GenerateFlashCardsWithAi
{
    internal static class GeneratedFlashCardCacheKeys
    {
        public static string GetAiRequestKey(int userId, int topicId)
        {
            return $"generatedFlashCards:aiRequest:{userId}:{topicId}";
        }

        public static string GetFlashCardsKey(int userId, int topicId)
        {
            return $"generatedFlashCards:flashCards:{userId}:{topicId}";
        }
    }
}
