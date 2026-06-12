using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Core.Commands.Note.UpdateNote.Request;
using StudyFlow.Core.Mapper;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;
using NoteEntity = StudyFlow.Domain.Entities.Note;

namespace StudyFlow.Core.Commands.Note.UpdateNote
{
    public class UpdateNoteCommandHandler : IRequestHandler<UpdateNoteCommand, Result<int>>
    {
        private readonly StudyFlowDbContext _dbContext;

        public UpdateNoteCommandHandler(StudyFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<int>> Handle(UpdateNoteCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.UpdateNoteDto.Title))
            {
                return Result<int>.Failure(NoteErrors.TitleRequired);
            }

            NoteEntity note = await _dbContext.Notes
                .FirstOrDefaultAsync(
                    x => x.Id == request.NoteId &&
                         x.Topic.Course.UserCourses.Any(userCourse => userCourse.UserId == request.UserId),
                    cancellationToken);

            if (note == null)
            {
                return Result<int>.Failure(NoteErrors.NoteNotFound);
            }

            request.UpdateNoteDto.MapToEntity(note);

            note.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result<int>.Success(note.Id);
        }
    }
}
