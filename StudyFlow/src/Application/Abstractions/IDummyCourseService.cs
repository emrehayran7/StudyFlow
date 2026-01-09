namespace StudyFlow.src.Application.Abstractions
{
    public interface IDummyCourseService
    {
        // Retrieves all dummy courses
        IEnumerable<DummyCourseDto> GetCourses();
        // Creates a new course and returns it
        DummyCourseDto CreateCourse(CreateDummyCourseDto dto);
    }

    public record DummyCourseDto(Guid Id, string Name);
    public record CreateDummyCourseDto(Guid Id, string Name);
}
