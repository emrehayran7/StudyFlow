using StudyFlow.src.Application.Abstractions;
using StudyFlow.src.Domain.Course;
using StudyFlow.src.Infrastructure;

namespace StudyFlow.src.Application
{
    public class DummySessionService : IDummySessionService
    {
        private readonly StudyFlowDbContext _context;

        public DummySessionService(StudyFlowDbContext context)
        {
            _context = context;
        }

        // Creates a new session record in the database
        public DummySessionDto CreateSession(CreateDummySessionDto dto)
        {
            var entity = new DummySession
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                StartTime = dto.StartTime,
                DurationMinutes = dto.DurationMinutes
            };

            _context.DummySessions.Add(entity);
            _context.SaveChanges();

            return new DummySessionDto(entity.Id, entity.Title, entity.StartTime, entity.DurationMinutes);
        }

        // Returns all sessions
        public IEnumerable<DummySessionDto> GetSessions()
        {
            return _context.DummySessions
                .Select(x => new DummySessionDto(x.Id, x.Title, x.StartTime, x.DurationMinutes))
                .ToList();
        }
    }
}
