using MediatR;
using StudyFlow.Core.Results;

namespace StudyFlow.Core.Commands.Note.DeleteNote.Request
{
    public record DeleteNoteCommand(int NoteId, int UserId) : IRequest<Result<bool>>;
}