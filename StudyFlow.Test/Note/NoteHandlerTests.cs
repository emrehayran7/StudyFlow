using FluentAssertions;
using StudyFlow.Core.Commands.Note.CreateNote;
using StudyFlow.Core.Commands.Note.CreateNote.Request;
using StudyFlow.Core.Commands.Note.DeleteNote;
using StudyFlow.Core.Commands.Note.DeleteNote.Request;
using StudyFlow.Core.Commands.Note.UpdateNote;
using StudyFlow.Core.Commands.Note.UpdateNote.Request;
using StudyFlow.Core.Queries.Note;
using StudyFlow.Core.Queries.Note.Response;
using StudyFlow.Core.Results;
using StudyFlow.Tests;
using CourseEntity = StudyFlow.Domain.Entities.Course;
using NoteEntity = StudyFlow.Domain.Entities.Note;
using TopicEntity = StudyFlow.Domain.Entities.Topic;
using UserCourseEntity = StudyFlow.Domain.Entities.UserCourse;

namespace StudyFlow.Tests.Note;

public class NoteHandlerTests
{
    [Fact]
    public async Task Create_Should_ReturnFailure_When_Title_Is_Empty()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var handler = new CreateNoteCommandHandler(dbContext);
        var command = new CreateNoteCommand(new CreateNoteDto
        {
            TopicId = 1,
            Title = "",
            Content = "Content"
        }, 1);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(NoteErrors.TitleRequired);
    }

    [Fact]
    public async Task Create_Should_Create_Note_When_Request_Is_Valid()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var topic = SeedTopic(dbContext);
        var handler = new CreateNoteCommandHandler(dbContext);
        var command = new CreateNoteCommand(new CreateNoteDto
        {
            TopicId = topic.Id,
            Title = "Note",
            Content = "Content"
        }, 1);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        dbContext.Notes.Should().ContainSingle(x => x.Id == result.Value && x.TopicId == topic.Id);
    }

    [Fact]
    public async Task Update_Should_Update_Note_When_Request_Is_Valid()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var topic = SeedTopic(dbContext);
        var note = new NoteEntity { TopicId = topic.Id, Title = "Old", Content = "Old content" };
        dbContext.Notes.Add(note);
        await dbContext.SaveChangesAsync();

        var handler = new UpdateNoteCommandHandler(dbContext);
        var command = new UpdateNoteCommand(note.Id, new UpdateNoteDto
        {
            Title = "Updated",
            Content = "Updated content"
        }, 1);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        note.Title.Should().Be("Updated");
        note.Content.Should().Be("Updated content");
        note.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task Delete_Should_Remove_Note_When_Note_Exists()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var topic = SeedTopic(dbContext);
        var note = new NoteEntity { TopicId = topic.Id, Title = "Note" };
        dbContext.Notes.Add(note);
        await dbContext.SaveChangesAsync();

        var handler = new DeleteNoteCommandHandler(dbContext);

        var result = await handler.Handle(new DeleteNoteCommand(note.Id, 1), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        dbContext.Notes.Should().BeEmpty();
    }

    [Fact]
    public async Task GetById_Should_Return_Note_When_Note_Exists()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var topic = SeedTopic(dbContext);
        var note = new NoteEntity { TopicId = topic.Id, Title = "Note", Content = "Content" };
        dbContext.Notes.Add(note);
        await dbContext.SaveChangesAsync();

        var handler = new GetNoteByIdQueryHandler(dbContext);

        var result = await handler.Handle(new GetNoteByIdQuery(note.Id, 1), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Id.Should().Be(note.Id);
        result.Value.Title.Should().Be("Note");
    }

    [Fact]
    public async Task GetAll_Should_Return_Notes_For_Topic()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var topic = SeedTopic(dbContext);
        dbContext.Notes.Add(new NoteEntity { TopicId = topic.Id, Title = "Note" });
        await dbContext.SaveChangesAsync();

        var handler = new GetNotesQueryHandler(dbContext);

        var result = await handler.Handle(new GetNotesQuery(topic.Id, 1), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(1);
    }

    private static TopicEntity SeedTopic(StudyFlow.Domain.Entities.StudyFlowDbContext dbContext)
    {
        var course = new CourseEntity { Title = "Course" };
        course.UserCourses.Add(new UserCourseEntity { UserId = 1 });
        var topic = new TopicEntity { Course = course, Title = "Topic", Status = "Pending" };
        dbContext.Topics.Add(topic);
        dbContext.SaveChanges();
        return topic;
    }
}
