using MediatR;
using StudyFlow.Core.Results;

namespace StudyFlow.Core.Commands.Note.UpdateNote.Request
{
    public record UpdateNoteCommand(int NoteId, UpdateNoteDto UpdateNoteDto, int UserId)
        : IRequest<Result<int>>;
}