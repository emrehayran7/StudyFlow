using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Core.Mapper;
using StudyFlow.Core.Queries.Note.Response;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;
using NoteEntity = StudyFlow.Domain.Entities.Note;
using StudyFlow.Core.Helper;

namespace StudyFlow.Core.Queries.Note
{
    public class GetNoteByIdQueryHandler : IRequestHandler<GetNoteByIdQuery, Result<GetNoteDto>>
    {
        private readonly StudyFlowDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public GetNoteByIdQueryHandler(StudyFlowDbContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }

        public async Task<Result<GetNoteDto>> Handle(GetNoteByIdQuery request, CancellationToken cancellationToken)
        {
            int userId = _currentUserService.GetUserId();

            NoteEntity note = await _dbContext.Notes
                .AsNoTracking()
                .Include(x => x.Topic)
                .FirstOrDefaultAsync(
                    x => x.Id == request.NoteId &&
                         x.Topic.Course.UserCourses.Any(userCourse => userCourse.UserId == userId),
                    cancellationToken);

            if (note == null)
            {
                return Result<GetNoteDto>.Failure(NoteErrors.NoteNotFound);
            }

            if (note.Topic == null)
            {
                return Result<GetNoteDto>.Failure(NoteErrors.TopicNotFound);
            }

            return Result<GetNoteDto>.Success(note.ToDto());
        }
    }
}
