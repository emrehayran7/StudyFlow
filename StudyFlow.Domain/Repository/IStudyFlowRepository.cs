using StudyFlow.Domain.Entities;

namespace StudyFlow.Domain.Repository
{
    public interface IStudyFlowRepository
    {
        Task<List<Course>> GetCoursesByUserIdAsync(int userId, CancellationToken cancellationToken);
        Task<List<Topic>> GetTopicsByCourseIdAsync(int courseId, CancellationToken cancellationToken);
        Task<List<Note>> GetNotesByTopicIdAsync(int topicId, CancellationToken cancellationToken);
        Task<List<FlashCard>> GetFlashCardsByTopicIdAsync(int topicId, CancellationToken cancellationToken);
        Task<List<StudySession>> GetStudySessionsByUserIdAsync(int userId, CancellationToken cancellationToken);
    }
}
