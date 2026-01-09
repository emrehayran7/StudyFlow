using StudyFlow.src.Application.Abstractions;
using StudyFlow.src.Domain.Course;
using StudyFlow.src.Infrastructure;
using System;

namespace StudyFlow.src.Application
{
    public class DummyCourseService : IDummyCourseService
    {
        private readonly StudyFlowDbContext _context;

        public DummyCourseService(StudyFlowDbContext context)
        {
            _context = context;
        }
        // Retrieves all dummy courses from the database
        public IEnumerable<DummyCourseDto> GetCourses()
        {
            return _context.DummyCourses
                .Select(x => new DummyCourseDto(x.Id, x.Name))
                .ToList();
        }

        // Creates a new course record in the database
        public DummyCourseDto CreateCourse(CreateDummyCourseDto dto)
        {
            var entity = new DummyCourse
            {
                Id = Guid.NewGuid(),
                Name = dto.Name
            };

            _context.DummyCourses.Add(entity);
            _context.SaveChanges();

            return new DummyCourseDto(entity.Id, entity.Name);
        }
    }
}
