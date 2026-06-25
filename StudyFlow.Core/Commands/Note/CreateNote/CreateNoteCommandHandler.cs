using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Core.Commands.Note.CreateNote.Request;
using StudyFlow.Core.Mapper;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;
using NoteEntity = StudyFlow.Domain.Entities.Note;
using StudyFlow.Core.Helper;

namespace StudyFlow.Core.Commands.Note.CreateNote
{
    public class CreateNoteCommandHandler : IRequestHandler<CreateNoteCommand, Result<int>>
    {
        private readonly StudyFlowDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public CreateNoteCommandHandler(StudyFlowDbContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }

        public async Task<Result<int>> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
        {
            int userId = _currentUserService.GetUserId();

            if (string.IsNullOrWhiteSpace(request.CreateNoteDto.Title))
            {
                return Result<int>.Failure(NoteErrors.TitleRequired);
            }

            bool topicExists = await _dbContext.Topics
                .AnyAsync(
                    x => x.Id == request.CreateNoteDto.TopicId &&
                         x.Course.UserCourses.Any(userCourse => userCourse.UserId == userId),
                    cancellationToken);

            if (!topicExists)
            {
                return Result<int>.Failure(NoteErrors.TopicNotFound);
            }

            NoteEntity note = request.CreateNoteDto.ToEntity();
            note.CreatedAt = DateTime.UtcNow;

            _dbContext.Notes.Add(note);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result<int>.Success(note.Id);
        }
    }
}
