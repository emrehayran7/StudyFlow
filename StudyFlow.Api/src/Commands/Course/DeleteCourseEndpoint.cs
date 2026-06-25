using MediatR;
using StudyFlow.Core.Commands.Course.DeleteCourse.Request;

namespace StudyFlow.Api.src.Commands.Course
{
    public static class DeleteCourseEndpoint
    {
        public static void DeleteCourse(this IEndpointRouteBuilder app)
        {
            app.MapDelete("/api/courses/", async (int courseId, IMediator mediator) =>
            {
                DeleteCourseCommand command = new DeleteCourseCommand(courseId);
                int result = await mediator.Send(command);

                return Results.Ok(result);
            })
            .WithName("DeleteCourse")
            .RequireAuthorization();
        }
    }
}
