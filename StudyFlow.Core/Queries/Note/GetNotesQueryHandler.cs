using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Core.Mapper;
using StudyFlow.Core.Queries.Note.Response;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;

namespace StudyFlow.Core.Queries.Note
{
    public class GetNotesQueryHandler : IRequestHandler<GetNotesQuery, Result<List<GetNoteDto>>>
    {
        private readonly StudyFlowDbContext _dbContext;

        public GetNotesQueryHandler(StudyFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<List<GetNoteDto>>> Handle(GetNotesQuery request, CancellationToken cancellationToken)
        {
            var notes = await _dbContext.Notes
                .AsNoTracking()
                .Where(x =>
                    x.TopicId == request.TopicId &&
                    x.Topic.Course.UserCourses.Any(userCourse => userCourse.UserId == request.UserId))
                .ToListAsync(cancellationToken);

            if (notes.Count == 0)
            {
                return Result<List<GetNoteDto>>.Failure(NoteErrors.NoteNotFound);
            }

            return Result<List<GetNoteDto>>.Success(notes.Select(x => x.ToDto()).ToList());
        }
    }
}
