using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Core.Commands.Note.DeleteNote.Request;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;
using NoteEntity = StudyFlow.Domain.Entities.Note;
using StudyFlow.Core.Helper;

namespace StudyFlow.Core.Commands.Note.DeleteNote
{
    public class DeleteNoteCommandHandler : IRequestHandler<DeleteNoteCommand, Result<bool>>
    {
        private readonly StudyFlowDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public DeleteNoteCommandHandler(StudyFlowDbContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }

        public async Task<Result<bool>> Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
        {
            int userId = _currentUserService.GetUserId();

            NoteEntity note = await _dbContext.Notes
                .FirstOrDefaultAsync(
                    x => x.Id == request.NoteId &&
                         x.Topic.Course.UserCourses.Any(userCourse => userCourse.UserId == userId),
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
