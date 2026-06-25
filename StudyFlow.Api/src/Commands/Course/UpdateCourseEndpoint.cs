using MediatR;
using StudyFlow.Core.Commands.Course.UpdateCourse.Request;
using StudyFlow.Core.Results;

namespace StudyFlow.Api.src.Commands.Course
{
    public static class UpdateCourseEndpoint
    {
        public static void UpdateCourse(this IEndpointRouteBuilder app)
        {
            app.MapPut("/api/courses/", async (int courseId, UpdateCourseDto dto, IMediator mediator) =>
            {
                UpdateCourseCommand command = new UpdateCourseCommand(courseId, dto);
                Result<int> result = await mediator.Send(command);

                if (result.IsSuccess)
                {
                    return Results.Ok(result.Value);
                }

                return result.Error.Type == ErrorType.NotFound
                    ? Results.NotFound(new
                    {
                        code = result.Error.Code,
                        result.Error.Description
                    })
                    : Results.BadRequest(new
                    {
                        code = result.Error.Code,
                        result.Error.Description
                    });
            })
            .WithName("UpdateCourse")
            .RequireAuthorization();
        }
    }
}
