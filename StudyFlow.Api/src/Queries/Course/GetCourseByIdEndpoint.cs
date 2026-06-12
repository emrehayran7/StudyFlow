using MediatR;
using StudyFlow.Api.src.Extensions;
using StudyFlow.Core.Queries.Course.Response;
using System.Security.Claims;

namespace StudyFlow.Api.src.Queries.Course
{
    public static class GetCourseByIdEndpoint
    {
        public static void GetCourseById(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/getcourseById", async (int CourseId, ClaimsPrincipal user, IMediator mediator) =>
            {
                int userId = user.GetUserId();
                GetCourseByIdQuery query = new GetCourseByIdQuery(userId, CourseId);
                GetCourseDto result = await mediator.Send(query);

                return Results.Ok(result);
            })
            .WithName("GetCourseById")
            .RequireAuthorization();
        }
    }
}
