using FluentAssertions;
using StudyFlow.Core.Commands.Topic.CreateTopic;
using StudyFlow.Core.Commands.Topic.CreateTopic.Request;
using StudyFlow.Core.Commands.Topic.DeleteTopic;
using StudyFlow.Core.Commands.Topic.DeleteTopic.Request;
using StudyFlow.Core.Commands.Topic.UpdateTopic;
using StudyFlow.Core.Commands.Topic.UpdateTopic.Request;
using StudyFlow.Core.Queries.Topic;
using StudyFlow.Core.Queries.Topic.Response;
using StudyFlow.Core.Results;
using StudyFlow.Tests;
using CourseEntity = StudyFlow.Domain.Entities.Course;
using TopicEntity = StudyFlow.Domain.Entities.Topic;
using UserCourseEntity = StudyFlow.Domain.Entities.UserCourse;

namespace StudyFlow.Tests.Topic;

public class TopicHandlerTests
{
    [Fact]
    public async Task Create_Should_ReturnFailure_When_Course_Does_Not_Exist()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var handler = new CreateTopicCommandHandler(dbContext, new FakeCurrentUserService(1));
        var command = new CreateTopicCommand(new CreateTopicDto
        {
            CourseId = 99,
            Title = "Topic",
            Status = "Pending"
        });

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(TopicErrors.CourseNotFound);
    }

    [Fact]
    public async Task Create_Should_Create_Topic_When_Request_Is_Valid()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var course = new CourseEntity { Title = "Course" };
        course.UserCourses.Add(new UserCourseEntity { UserId = 1 });
        dbContext.Courses.Add(course);
        await dbContext.SaveChangesAsync();

        var handler = new CreateTopicCommandHandler(dbContext, new FakeCurrentUserService(1));
        var command = new CreateTopicCommand(new CreateTopicDto
        {
            CourseId = course.Id,
            Title = "Topic",
            Description = "Description",
            Status = "Pending",
            PriorityLevel = 2
        });

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        dbContext.Topics.Should().ContainSingle(x => x.Id == result.Value && x.CourseId == course.Id);
    }

    [Fact]
    public async Task Update_Should_ReturnFailure_When_Topic_Does_Not_Exist()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var handler = new UpdateTopicHandler(dbContext, new FakeCurrentUserService(1));
        var command = new UpdateTopicCommand(99, new UpdateTopicDto
        {
            CourseId = 1,
            Title = "Updated",
            Status = "InProgress"
        });

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(TopicErrors.TopicNotFound);
    }

    [Fact]
    public async Task Update_Should_Update_Topic_When_Request_Is_Valid()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var course = new CourseEntity { Title = "Course" };
        course.UserCourses.Add(new UserCourseEntity { UserId = 1 });
        var topic = new TopicEntity { Course = course, Title = "Old", Status = "Pending" };
        dbContext.Topics.Add(topic);
        await dbContext.SaveChangesAsync();

        var handler = new UpdateTopicHandler(dbContext, new FakeCurrentUserService(1));
        var command = new UpdateTopicCommand(topic.Id, new UpdateTopicDto
        {
            CourseId = course.Id,
            Title = "Updated",
            Description = "Updated description",
            Status = "Done",
            PriorityLevel = 5
        });

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        topic.Title.Should().Be("Updated");
        topic.Description.Should().Be("Updated description");
        topic.Status.Should().Be("Done");
        topic.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task Delete_Should_Remove_Topic_When_Topic_Exists()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var course = new CourseEntity { Title = "Course" };
        course.UserCourses.Add(new UserCourseEntity { UserId = 1 });
        var topic = new TopicEntity { Course = course, Title = "Topic", Status = "Pending" };
        dbContext.Topics.Add(topic);
        await dbContext.SaveChangesAsync();

        var handler = new DeleteTopicCommandHandler(dbContext, new FakeCurrentUserService(1));

        var result = await handler.Handle(new DeleteTopicCommand(topic.Id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        dbContext.Topics.Should().BeEmpty();
    }

    [Fact]
    public async Task GetById_Should_Return_Topic_When_Topic_Exists()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var course = new CourseEntity { Title = "Course" };
        course.UserCourses.Add(new UserCourseEntity { UserId = 1 });
        var topic = new TopicEntity { Course = course, Title = "Topic", Status = "Pending" };
        dbContext.Topics.Add(topic);
        await dbContext.SaveChangesAsync();

        var handler = new GetTopicByIdQueryHandler(dbContext, new FakeCurrentUserService(1));

        var result = await handler.Handle(new GetTopicByIdQuery(topic.Id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Id.Should().Be(topic.Id);
        result.Value.Title.Should().Be("Topic");
    }

    [Fact]
    public async Task GetAll_Should_Return_Topics_For_Course()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var course = new CourseEntity { Title = "Course" };
        course.UserCourses.Add(new UserCourseEntity { UserId = 1 });
        dbContext.Topics.Add(new TopicEntity { Course = course, Title = "Topic", Status = "Pending" });
        await dbContext.SaveChangesAsync();

        var handler = new GetTopicsQueryHandler(dbContext, new FakeCurrentUserService(1));

        var result = await handler.Handle(new GetTopicsQuery(course.Id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(1);
    }
}
