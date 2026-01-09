namespace StudyFlow.src.Application.Abstractions
{
    public interface IDummySessionService
    {
        IEnumerable<DummySessionDto> GetSessions();
        DummySessionDto CreateSession(CreateDummySessionDto dto);

    }

    public record DummySessionDto(Guid Id, string Title, DateTime StartTime, int DurationMinutes);
    public record CreateDummySessionDto(string Title, DateTime StartTime, int DurationMinutes);



}
