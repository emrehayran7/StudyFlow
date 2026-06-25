using MediatR;
using StudyFlow.Core.Queries.Course.Response;

namespace StudyFlow.Api.src.Queries.Course
{
    public static class GetCourseByIdEndpoint
    {
        public static void GetCourseById(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/getcourseById", async (int CourseId, IMediator mediator) =>
            {
                GetCourseByIdQuery query = new GetCourseByIdQuery(CourseId);
                GetCourseDto result = await mediator.Send(query);

                return Results.Ok(result);
            })
            .WithName("GetCourseById")
            .RequireAuthorization();
        }
    }
}
