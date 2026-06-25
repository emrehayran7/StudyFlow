using MediatR;
using StudyFlow.Core.Results;

namespace StudyFlow.Core.Queries.Note.Response
{
    public record GetNotesQuery(int TopicId) : IRequest<Result<List<GetNoteDto>>>;
}
