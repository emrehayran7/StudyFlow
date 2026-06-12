using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Core.Commands.StudySession.CreateStudySession.Request;
using StudyFlow.Core.Mapper;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;
using StudySessionEntity = StudyFlow.Domain.Entities.StudySession;

namespace StudyFlow.Core.Commands.StudySession.CreateStudySession
{
    public class CreateStudySessionCommandHandler : IRequestHandler<CreateStudySessionCommand, Result<int>>
    {
        private readonly StudyFlowDbContext _dbContext;

        public CreateStudySessionCommandHandler(StudyFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<int>> Handle(CreateStudySessionCommand request, CancellationToken cancellationToken)
        {
            var dto = request.CreateStudySessionDto;

            if (dto.DurationMinutes <= 0)
            {
                return Result<int>.Failure(StudySessionErrors.InvalidDuration);
            }

            bool topicExists = await _dbContext.Topics
                .AnyAsync(
                    x => x.Id == dto.TopicId &&
                         x.Course.UserCourses.Any(userCourse => userCourse.UserId == request.UserId),
                    cancellationToken);

            if (!topicExists)
            {
                return Result<int>.Failure(StudySessionErrors.TopicNotFound);
            }

            StudySessionEntity session = dto.ToEntity(request.UserId);

         
            session.EndTime = dto.StartTime.AddMinutes(dto.DurationMinutes);

            _dbContext.StudySessions.Add(session);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result<int>.Success(session.Id);
        }
    }
}
