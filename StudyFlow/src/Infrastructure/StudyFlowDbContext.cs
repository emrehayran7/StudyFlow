using Microsoft.EntityFrameworkCore;
using StudyFlow.src.Domain.Course;

namespace StudyFlow.src.Infrastructure
{
    public class StudyFlowDbContext : DbContext
    {
        public StudyFlowDbContext(DbContextOptions<StudyFlowDbContext> options)
            : base(options)
        {
        }

        public DbSet<Course> Course { get; set; }
        public DbSet<DummyCourse> DummyCourses { get; set; }
        public DbSet<DummySession> DummySessions { get; set; }

    }
}
