using MediatR;
using StudyFlow.Api.src.Extensions;
using StudyFlow.Core.Commands.Course.UpdateCourse.Request;
using StudyFlow.Core.Results;
using System.Security.Claims;

namespace StudyFlow.Api.src.Commands.Course
{
    public static class UpdateCourseEndpoint
    {
        public static void UpdateCourse(this IEndpointRouteBuilder app)
        {
            app.MapPut("/api/courses/", async (int courseId, UpdateCourseDto dto, ClaimsPrincipal user, IMediator mediator) =>
            {
                int userId = user.GetUserId();
                UpdateCourseCommand command = new UpdateCourseCommand(courseId, dto, userId);
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
