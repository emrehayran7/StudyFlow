using MediatR;
using StudyFlow.Core.Results;

namespace StudyFlow.Core.Commands.Note.DeleteNote.Request
{
    public record DeleteNoteCommand(int NoteId) : IRequest<Result<bool>>;
}