using MediatR;
using StudyFlow.Core.Queries.Note.Response;
using StudyFlow.Core.Results;

namespace StudyFlow.Core.Queries.Note
{
    public record GetNoteByIdQuery(int NoteId, int UserId) : IRequest<Result<GetNoteDto>>;
}
