using MediatR;
using StudyFlow.Core.Results;

namespace StudyFlow.Core.Queries.Note.Response
{
    public record GetNotesQuery(int TopicId, int UserId) : IRequest<Result<List<GetNoteDto>>>;
}
