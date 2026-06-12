using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Core.Mapper;
using StudyFlow.Core.Queries.Note.Response;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;
using NoteEntity = StudyFlow.Domain.Entities.Note;

namespace StudyFlow.Core.Queries.Note
{
    public class GetNoteByIdQueryHandler : IRequestHandler<GetNoteByIdQuery, Result<GetNoteDto>>
    {
        private readonly StudyFlowDbContext _dbContext;

        public GetNoteByIdQueryHandler(StudyFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<GetNoteDto>> Handle(GetNoteByIdQuery request, CancellationToken cancellationToken)
        {
            NoteEntity note = await _dbContext.Notes
                .AsNoTracking()
                .Include(x => x.Topic)
                .FirstOrDefaultAsync(
                    x => x.Id == request.NoteId &&
                         x.Topic.Course.UserCourses.Any(userCourse => userCourse.UserId == request.UserId),
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
