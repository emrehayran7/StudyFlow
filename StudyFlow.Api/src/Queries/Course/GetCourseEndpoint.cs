using MediatR;
using StudyFlow.Api.src.Extensions;
using StudyFlow.Core.Queries.Course.Response;
using System.Security.Claims;

namespace StudyFlow.Api.src.Queries.Course
{
    public static class GetCourseEndpoint
    {
        public static void GetCourse(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/getcourses", async (ClaimsPrincipal user, IMediator mediator) =>
            {
                int userId = user.GetUserId();
                GetCoursesQuery query = new GetCoursesQuery(userId);
                List<GetCourseDto> result = await mediator.Send(query);

                return Results.Ok(result);
            })
            .WithName("GetCourses")
            .RequireAuthorization();
        }
    }
}
