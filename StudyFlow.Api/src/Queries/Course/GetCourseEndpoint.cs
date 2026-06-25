using MediatR;
using StudyFlow.Core.Queries.Course.Response;

namespace StudyFlow.Api.src.Queries.Course
{
    public static class GetCourseEndpoint
    {
        public static void GetCourse(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/getcourses", async (IMediator mediator) =>
            {
                GetCoursesQuery query = new GetCoursesQuery();
                List<GetCourseDto> result = await mediator.Send(query);

                return Results.Ok(result);
            })
            .WithName("GetCourses")
            .RequireAuthorization();
        }
    }
}
