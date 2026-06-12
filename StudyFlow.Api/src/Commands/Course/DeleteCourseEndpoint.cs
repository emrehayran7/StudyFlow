using MediatR;
using StudyFlow.Api.src.Extensions;
using StudyFlow.Core.Commands.Course.DeleteCourse.Request;
using System.Security.Claims;

namespace StudyFlow.Api.src.Commands.Course
{
    public static class DeleteCourseEndpoint
    {
        public static void DeleteCourse(this IEndpointRouteBuilder app)
        {
            app.MapDelete("/api/courses/", async (int courseId, ClaimsPrincipal user, IMediator mediator) =>
            {
                int userId = user.GetUserId();
                DeleteCourseCommand command = new DeleteCourseCommand(courseId, userId);
                int result = await mediator.Send(command);

                return Results.Ok(result);
            })
            .WithName("DeleteCourse")
            .RequireAuthorization();
        }
    }
}
