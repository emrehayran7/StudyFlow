using Microsoft.EntityFrameworkCore;
using StudyFlow.Domain.Entities;

namespace StudyFlow.Domain.Repository
{
    public class StudyFlowRepository : IStudyFlowRepository
    {
        private readonly StudyFlowDbContext _dbContext;

        public StudyFlowRepository(StudyFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<Course>> GetCoursesByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            return _dbContext.UserCourses
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(x => x.Course)
                .ToListAsync(cancellationToken);
        }

        public Task<List<Topic>> GetTopicsByCourseIdAsync(int courseId, CancellationToken cancellationToken)
        {
            return _dbContext.Topics
                .AsNoTracking()
                .Where(x => x.CourseId == courseId)
                .ToListAsync(cancellationToken);
        }

        public Task<List<Note>> GetNotesByTopicIdAsync(int topicId, CancellationToken cancellationToken)
        {
            return _dbContext.Notes
                .AsNoTracking()
                .Where(x => x.TopicId == topicId)
                .ToListAsync(cancellationToken);
        }

        public Task<List<FlashCard>> GetFlashCardsByTopicIdAsync(int topicId, CancellationToken cancellationToken)
        {
            return _dbContext.FlashCards
                .AsNoTracking()
                .Where(x => x.TopicId == topicId)
                .ToListAsync(cancellationToken);
        }

        public Task<List<StudySession>> GetStudySessionsByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            return _dbContext.StudySessions
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .ToListAsync(cancellationToken);
        }
    }
}
