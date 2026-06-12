using StudyFlow.Core.Commands.Note.CreateNote.Request;
using StudyFlow.Core.Commands.Note.UpdateNote.Request;
using StudyFlow.Core.Queries.Note.Response;
using StudyFlow.Domain.Entities;

namespace StudyFlow.Core.Mapper
{
    public static class NoteMapper
    {
        public static Note ToEntity(this CreateNoteDto dto)
        {
            return new Note
            {
                TopicId = dto.TopicId,
                Title = dto.Title,
                Content = dto.Content
            };
        }

        public static void MapToEntity(this UpdateNoteDto dto, Note note)
        {
            note.Title = dto.Title;
            note.Content = dto.Content;
        }

        public static GetNoteDto ToDto(this Note note)
        {
            return new GetNoteDto
            {
                Id = note.Id,
                TopicId = note.TopicId,
                Title = note.Title,
                Content = note.Content,
                CreatedAt = note.CreatedAt,
                UpdatedAt = note.UpdatedAt
            };
        }
    }
}