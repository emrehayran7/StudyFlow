using MediatR;
using StudyFlow.Core.Queries.AiRequest.Response;
using StudyFlow.Core.Results;

namespace StudyFlow.Api.src.Queries.AiRequest
{
    public static class GetAiRequestByIdEndpoint
    {
        public static void GetAiRequestById(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/airequests/{aiRequestId:int}", async (int aiRequestId, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetAiRequestByIdQuery(aiRequestId));

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
            .WithName("GetAiRequestById")
            .RequireAuthorization();
        }
    }
}
