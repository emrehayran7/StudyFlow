using MediatR;
using StudyFlow.Core.Commands.Course.CreateCourse.Request;
using StudyFlow.Core.Results;

namespace StudyFlow.Api.src.Commands.Course
{
    public static class CreateCourseEndpoint
    {
        public static void CreateCourse(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/createcourse", async (CreateCourseDto dto, IMediator mediator) =>
            {
                CreateCourseCommand command = new CreateCourseCommand(dto);

                Result<int> result = await mediator.Send(command);

                if (result.IsSuccess)
                {
                    return Results.Ok(result.Value);
                }

                return Results.BadRequest(new { code = result.Error.Code, result.Error.Description });
            })
            .WithName("CreateCourse")
            .RequireAuthorization();
        }
    }
}
