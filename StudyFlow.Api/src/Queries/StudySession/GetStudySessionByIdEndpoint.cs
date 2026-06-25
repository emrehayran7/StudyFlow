using MediatR;
using StudyFlow.Core.Queries.StudySession;
using StudyFlow.Core.Results;

namespace StudyFlow.Api.src.Queries.StudySession
{
    public static class GetStudySessionByIdEndpoint
    {
        public static void GetStudySessionById(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/studysessions/{id:int}", async (int id, IMediator mediator) =>
            {
                var query = new GetStudySessionByIdQuery(id);

                var result = await mediator.Send(query);

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
            .WithName("GetStudySessionById")
            .RequireAuthorization();
        }
    }
}
