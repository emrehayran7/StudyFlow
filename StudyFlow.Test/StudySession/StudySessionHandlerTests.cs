using FluentAssertions;
using StudyFlow.Core.Commands.StudySession.CreateStudySession;
using StudyFlow.Core.Commands.StudySession.CreateStudySession.Request;
using StudyFlow.Core.Commands.StudySession.DeleteStudySession;
using StudyFlow.Core.Commands.StudySession.DeleteStudySession.Request;
using StudyFlow.Core.Commands.StudySession.UpdateStudySession;
using StudyFlow.Core.Commands.StudySession.UpdateStudySession.Request;
using StudyFlow.Core.Queries.StudySession;
using StudyFlow.Core.Queries.StudySession.Response;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Repository;
using CourseEntity = StudyFlow.Domain.Entities.Course;
using StudySessionEntity = StudyFlow.Domain.Entities.StudySession;
using TopicEntity = StudyFlow.Domain.Entities.Topic;
using UserCourseEntity = StudyFlow.Domain.Entities.UserCourse;
using UserEntity = StudyFlow.Domain.Entities.User;

namespace StudyFlow.Tests.StudySession;

public class StudySessionHandlerTests
{
    [Fact]
    public async Task Create_Should_ReturnFailure_When_Duration_Is_Invalid()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var handler = new CreateStudySessionCommandHandler(dbContext);
        var command = new CreateStudySessionCommand(new CreateStudySessionDto
        {
            TopicId = 1,
            StartTime = DateTime.UtcNow,
            DurationMinutes = 0
        }, 1);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(StudySessionErrors.InvalidDuration);
    }

    [Fact]
    public async Task Create_Should_Create_StudySession_When_Request_Is_Valid()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var topic = SeedTopic(dbContext, 12);
        var startTime = DateTime.UtcNow;
        var handler = new CreateStudySessionCommandHandler(dbContext);
        var command = new CreateStudySessionCommand(new CreateStudySessionDto
        {
            TopicId = topic.Id,
            StartTime = startTime,
            DurationMinutes = 30,
            SessionNotes = "Notes"
        }, 12);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        dbContext.StudySessions.Should().ContainSingle(x =>
            x.Id == result.Value &&
            x.UserId == 12 &&
            x.EndTime == startTime.AddMinutes(30));
    }

    [Fact]
    public async Task Update_Should_Update_StudySession_When_Request_Is_Valid()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var topic = SeedTopic(dbContext, 4);
        var session = new StudySessionEntity
        {
            TopicId = topic.Id,
            UserId = 4,
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow.AddMinutes(15),
            DurationMinutes = 15
        };
        dbContext.StudySessions.Add(session);
        await dbContext.SaveChangesAsync();

        var startTime = DateTime.UtcNow.AddHours(1);
        var handler = new UpdateStudySessionCommandHandler(dbContext);
        var command = new UpdateStudySessionCommand(session.Id, new UpdateStudySessionDto
        {
            StartTime = startTime,
            DurationMinutes = 45,
            SessionNotes = "Updated"
        }, 4);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        session.DurationMinutes.Should().Be(45);
        session.EndTime.Should().Be(startTime.AddMinutes(45));
        session.SessionNotes.Should().Be("Updated");
    }

    [Fact]
    public async Task Delete_Should_Remove_StudySession_When_StudySession_Exists()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var topic = SeedTopic(dbContext, 4);
        var session = new StudySessionEntity
        {
            TopicId = topic.Id,
            UserId = 4,
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow.AddMinutes(15),
            DurationMinutes = 15
        };
        dbContext.StudySessions.Add(session);
        await dbContext.SaveChangesAsync();

        var handler = new DeleteStudySessionCommandHandler(dbContext);

        var result = await handler.Handle(new DeleteStudySessionCommand(session.Id, 4), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        dbContext.StudySessions.Should().BeEmpty();
    }

    [Fact]
    public async Task GetById_Should_Return_StudySession_When_StudySession_Exists()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var topic = SeedTopic(dbContext, 4);
        var session = new StudySessionEntity
        {
            TopicId = topic.Id,
            UserId = 4,
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow.AddMinutes(15),
            DurationMinutes = 15
        };
        dbContext.StudySessions.Add(session);
        await dbContext.SaveChangesAsync();

        var handler = new GetStudySessionByIdQueryHandler(dbContext);

        var result = await handler.Handle(new GetStudySessionByIdQuery(session.Id, 4), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Id.Should().Be(session.Id);
        result.Value.UserId.Should().Be(4);
    }

    [Fact]
    public async Task GetAll_Should_Return_StudySessions_For_User()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var topic = SeedTopic(dbContext, 4);
        dbContext.StudySessions.Add(new StudySessionEntity
        {
            TopicId = topic.Id,
            UserId = 4,
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow.AddMinutes(15),
            DurationMinutes = 15
        });
        await dbContext.SaveChangesAsync();

        var repository = new StudyFlowRepository(dbContext);
        var handler = new GetStudySessionsQueryHandler(repository);

        var result = await handler.Handle(new GetStudySessionsQuery(4), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(1);
    }

    private static TopicEntity SeedTopic(StudyFlow.Domain.Entities.StudyFlowDbContext dbContext, int userId)
    {
        var user = new UserEntity
        {
            Id = userId,
            FirstName = "Test",
            LastName = "User",
            Email = "test@example.com",
            Password = [1, 2, 3]
        };
        var course = new CourseEntity { Title = "Course" };
        course.UserCourses.Add(new UserCourseEntity { UserId = userId });
        var topic = new TopicEntity { Course = course, Title = "Topic", Status = "Pending" };
        dbContext.Users.Add(user);
        dbContext.Topics.Add(topic);
        dbContext.SaveChanges();
        return topic;
    }
}
