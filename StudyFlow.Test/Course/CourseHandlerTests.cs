using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Core.Commands.Course.CreateCourse;
using StudyFlow.Core.Commands.Course.CreateCourse.Request;
using StudyFlow.Core.Commands.Course.DeleteCourse;
using StudyFlow.Core.Commands.Course.DeleteCourse.Request;
using StudyFlow.Core.Commands.Course.UpdateCourse;
using StudyFlow.Core.Commands.Course.UpdateCourse.Request;
using StudyFlow.Core.Queries.Course;
using StudyFlow.Core.Queries.Course.Response;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;
using StudyFlow.Domain.Repository;
using StudyFlow.Tests;
using CourseEntity = StudyFlow.Domain.Entities.Course;

namespace StudyFlow.Tests.Course;

public class CourseHandlerTests
{
    [Fact]
    public async Task Create_Should_ReturnFailure_When_Title_Is_Empty()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var handler = new CreateCourseCommandHandler(dbContext, new FakeCurrentUserService(1));
        var command = new CreateCourseCommand(new CreateCourseDto
        {
            Title = "",
            Description = "Test description"
        });

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(CourseErrors.TitleRequired);
        dbContext.Courses.Should().BeEmpty();
    }

    [Fact]
    public async Task Create_Should_Create_Course_When_Title_Is_Valid()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var handler = new CreateCourseCommandHandler(dbContext, new FakeCurrentUserService(5));
        var command = new CreateCourseCommand(new CreateCourseDto
        {
            Title = "Math",
            Description = "Math course"
        });

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeGreaterThan(0);

        var course = await dbContext.Courses
            .Include(x => x.UserCourses)
            .FirstOrDefaultAsync();

        course.Should().NotBeNull();
        course!.Title.Should().Be("Math");
        course.Description.Should().Be("Math course");
        course.CreatedAt.Should().NotBeNull();
        course.UserCourses.Should().HaveCount(1);
        course.UserCourses.First().UserId.Should().Be(5);
    }

    [Fact]
    public async Task Update_Should_ReturnFailure_When_Course_Does_Not_Exist()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var handler = new UpdateCourseCommandHandler(dbContext, new FakeCurrentUserService(1));
        var command = new UpdateCourseCommand(99, new UpdateCourseDto
        {
            Title = "Updated",
            Description = "Updated description"
        });

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(CourseErrors.CourseNotFound);
    }

    [Fact]
    public async Task Update_Should_Update_Course_When_Request_Is_Valid()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var course = new CourseEntity { Title = "Old", Description = "Old description" };
        course.UserCourses.Add(new UserCourse { UserId = 1 });
        dbContext.Courses.Add(course);
        await dbContext.SaveChangesAsync();

        var handler = new UpdateCourseCommandHandler(dbContext, new FakeCurrentUserService(1));
        var command = new UpdateCourseCommand(course.Id, new UpdateCourseDto
        {
            Title = "Updated",
            Description = "Updated description"
        });

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        course.Title.Should().Be("Updated");
        course.Description.Should().Be("Updated description");
        course.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task Delete_Should_Remove_Course_And_UserCourses()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var course = new CourseEntity { Title = "Course" };
        dbContext.Courses.Add(course);
        await dbContext.SaveChangesAsync();
        dbContext.UserCourses.Add(new UserCourse { CourseId = course.Id, UserId = 7 });
        await dbContext.SaveChangesAsync();

        var handler = new DeleteCourseCommandHandler(dbContext, new FakeCurrentUserService(7));

        var result = await handler.Handle(new DeleteCourseCommand(course.Id), CancellationToken.None);

        result.Should().Be(course.Id);
        dbContext.Courses.Should().BeEmpty();
        dbContext.UserCourses.Should().BeEmpty();
    }

    [Fact]
    public async Task GetById_Should_Return_Course_For_User()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var course = new CourseEntity { Title = "Course", Description = "Description" };
        dbContext.Courses.Add(course);
        await dbContext.SaveChangesAsync();
        dbContext.UserCourses.Add(new UserCourse { CourseId = course.Id, UserId = 3 });
        await dbContext.SaveChangesAsync();

        var handler = new GetCourseByIdQueryHandler(dbContext, new FakeCurrentUserService(3));

        var result = await handler.Handle(new GetCourseByIdQuery(course.Id), CancellationToken.None);

        result.Id.Should().Be(course.Id);
        result.Title.Should().Be("Course");
    }

    [Fact]
    public async Task GetAll_Should_Return_Courses_For_User()
    {
        await using var dbContext = TestDbContextFactory.Create();
        var course = new CourseEntity { Title = "Course", Description = "Description" };
        dbContext.Courses.Add(course);
        await dbContext.SaveChangesAsync();
        dbContext.UserCourses.Add(new UserCourse { CourseId = course.Id, UserId = 3 });
        await dbContext.SaveChangesAsync();

        var repository = new StudyFlowRepository(dbContext);
        var handler = new GetCourseQueryHandler(repository, new FakeCurrentUserService(3));

        var result = await handler.Handle(new GetCoursesQuery(), CancellationToken.None);

        result.Should().HaveCount(1);
        result[0].Id.Should().Be(course.Id);
    }
}
