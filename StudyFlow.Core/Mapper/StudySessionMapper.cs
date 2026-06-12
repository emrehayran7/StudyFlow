using StudyFlow.Core.Commands.StudySession.CreateStudySession.Request;
using StudyFlow.Core.Commands.StudySession.UpdateStudySession.Request;
using StudyFlow.Core.Queries.StudySession.Response;
using StudyFlow.Domain.Entities;

namespace StudyFlow.Core.Mapper
{
    public static class StudySessionMapper
    {
        public static StudySession ToEntity(this CreateStudySessionDto dto, int userId)
        {
            return new StudySession
            {
                TopicId = dto.TopicId,
                UserId = userId,
                StartTime = dto.StartTime,
                DurationMinutes = dto.DurationMinutes,
                SessionNotes = dto.SessionNotes
            };
        }

        public static void MapToEntity(this UpdateStudySessionDto dto, StudySession entity)
        {
            entity.StartTime = dto.StartTime;
            entity.DurationMinutes = dto.DurationMinutes;
            entity.SessionNotes = dto.SessionNotes;
        }

        public static GetStudySessionDto ToDto(this StudySession entity)
        {
            return new GetStudySessionDto
            {
                Id = entity.Id,
                UserId = entity.UserId,
                TopicId = entity.TopicId,
                StartTime = entity.StartTime,
                EndTime = entity.EndTime,
                DurationMinutes = entity.DurationMinutes,
                SessionNotes = entity.SessionNotes
            };
        }
    }
}