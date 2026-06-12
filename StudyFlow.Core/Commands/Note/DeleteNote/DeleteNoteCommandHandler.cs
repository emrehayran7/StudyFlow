using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Core.Commands.Note.DeleteNote.Request;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;
using NoteEntity = StudyFlow.Domain.Entities.Note;

namespace StudyFlow.Core.Commands.Note.DeleteNote
{
    public class DeleteNoteCommandHandler : IRequestHandler<DeleteNoteCommand, Result<bool>>
    {
        private readonly StudyFlowDbContext _dbContext;

        public DeleteNoteCommandHandler(StudyFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<bool>> Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
        {
            NoteEntity note = await _dbContext.Notes
                .FirstOrDefaultAsync(
                    x => x.Id == request.NoteId &&
                         x.Topic.Course.UserCourses.Any(userCourse => userCourse.UserId == request.UserId),
                    cancellationToken);

            if (note == null)
            {
                return Result<bool>.Failure(NoteErrors.NoteNotFound);
            }

            _dbContext.Notes.Remove(note);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
