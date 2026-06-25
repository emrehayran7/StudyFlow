using FluentAssertions;
using StudyFlow.Core.Commands.FlashCard.CreateFlashCard;
using StudyFlow.Core.Commands.FlashCard.CreateFlashCard.Request;
using StudyFlow.Core.Commands.FlashCard.DeleteFlashCard;
using StudyFlow.Core.Commands.FlashCard.DeleteFlashCard.Request;
using StudyFlow.Core.Commands.FlashCard.UpdateFlashCard;
using StudyFlow.Core.Commands.FlashCard.UpdateFlashCard.Request;
using StudyFlow.Core.Queries.FlashCard;
using StudyFlow.Core.Queries.FlashCard.Response;
using StudyFlow.Core.Results;
using StudyFlow.Tests;
using CourseEntity = StudyFlow.Domain.Entities.Course;
using FlashCardEntity = StudyFlow.Domain.Entities.FlashCard;
using TopicEntity = StudyFlow.Domain.Entities.Topic;
using UserCourseEntity = StudyFlow.Domain.Entities.UserCourse;

namespace StudyFlow.Tests.FlashCard;

public class FlashCardHandlerTests
{
    [Fact]
    public async Task Create_Should_ReturnFailure_When_Question_Is_Empty()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var handler = new CreateFlashCardCommandHandler(dbContext, new FakeCurrentUserService(1));
        var command = new CreateFlashCardCommand(new CreateFlashCardDto
        {
            TopicId = 1,
            Question = "",
            Answer = "Answer"
        });

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(FlashCardErrors.QuestionRequired);
    }

    [Fact]
    public async Task Create_Should_Create_FlashCard_When_Request_Is_Valid()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var topic = SeedTopic(dbContext, 9);
        var handler = new CreateFlashCardCommandHandler(dbContext, new FakeCurrentUserService(9));
        var command = new CreateFlashCardCommand(new CreateFlashCardDto
        {
            TopicId = topic.Id,
            Question = "Question",
            Answer = "Answer",
            Hint = "Hint",
            DifficultyLevel = 2
        });

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        dbContext.FlashCards.Should().ContainSingle(x =>
            x.Id == result.Value &&
            x.TopicId == topic.Id &&
            x.CreatedBy == "9");
    }

    [Fact]
    public async Task Update_Should_Update_FlashCard_When_Request_Is_Valid()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var topic = SeedTopic(dbContext, 8);
        var flashCard = new FlashCardEntity
        {
            TopicId = topic.Id,
            Question = "Old question",
            Answer = "Old answer",
            Hint = "Old",
            DifficultyLevel = 1
        };
        dbContext.FlashCards.Add(flashCard);
        await dbContext.SaveChangesAsync();

        var handler = new UpdateFlashCardCommandHandler(dbContext, new FakeCurrentUserService(8));
        var command = new UpdateFlashCardCommand(flashCard.Id, new UpdateFlashCardDto
        {
            Question = "Updated question",
            Answer = "Updated answer",
            Hint = "Updated",
            DifficultyLevel = 4
        });

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        flashCard.Question.Should().Be("Updated question");
        flashCard.Answer.Should().Be("Updated answer");
        flashCard.UpdatedBy.Should().Be("8");
        flashCard.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task Delete_Should_Remove_FlashCard_When_FlashCard_Exists()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var topic = SeedTopic(dbContext);
        var flashCard = new FlashCardEntity { TopicId = topic.Id, Question = "Question", Answer = "Answer", Hint = "Hint" };
        dbContext.FlashCards.Add(flashCard);
        await dbContext.SaveChangesAsync();

        var handler = new DeleteFlashCardCommandHandler(dbContext, new FakeCurrentUserService(1));

        var result = await handler.Handle(new DeleteFlashCardCommand(flashCard.Id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        dbContext.FlashCards.Should().BeEmpty();
    }

    [Fact]
    public async Task GetById_Should_Return_FlashCard_When_FlashCard_Exists()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var topic = SeedTopic(dbContext);
        var flashCard = new FlashCardEntity { TopicId = topic.Id, Question = "Question", Answer = "Answer", Hint = "Hint" };
        dbContext.FlashCards.Add(flashCard);
        await dbContext.SaveChangesAsync();

        var handler = new GetFlashCardByIdQueryHandler(dbContext, new FakeCurrentUserService(1));

        var result = await handler.Handle(new GetFlashCardByIdQuery(flashCard.Id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Id.Should().Be(flashCard.Id);
        result.Value.Question.Should().Be("Question");
    }

    [Fact]
    public async Task GetAll_Should_Return_FlashCards_For_Topic()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var topic = SeedTopic(dbContext);
        dbContext.FlashCards.Add(new FlashCardEntity { TopicId = topic.Id, Question = "Question", Answer = "Answer", Hint = "Hint" });
        await dbContext.SaveChangesAsync();

        var handler = new GetFlashCardsQueryHandler(dbContext, new FakeCurrentUserService(1));

        var result = await handler.Handle(new GetFlashCardsQuery(topic.Id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(1);
    }

    private static TopicEntity SeedTopic(StudyFlow.Domain.Entities.StudyFlowDbContext dbContext, int userId = 1)
    {
        var course = new CourseEntity { Title = "Course" };
        course.UserCourses.Add(new UserCourseEntity { UserId = userId });
        var topic = new TopicEntity { Course = course, Title = "Topic", Status = "Pending" };
        dbContext.Topics.Add(topic);
        dbContext.SaveChanges();
        return topic;
    }
}
