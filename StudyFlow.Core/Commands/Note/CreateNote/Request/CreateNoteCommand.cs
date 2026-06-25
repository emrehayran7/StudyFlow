using MediatR;
using StudyFlow.Core.Results;

namespace StudyFlow.Core.Commands.Note.CreateNote.Request
{
    public record CreateNoteCommand(CreateNoteDto CreateNoteDto) : IRequest<Result<int>>;
}