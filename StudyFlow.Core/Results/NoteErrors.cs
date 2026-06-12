using System;

namespace StudyFlow.Core.Results
{
    public static class NoteErrors
    {
        public static readonly Error TitleRequired =
            new Error("Note.TitleRequired", "Note title cannot be null or empty.", ErrorType.Validation);

        public static readonly Error TopicNotFound =
            new Error("Note.TopicNotFound", "Topic not found.", ErrorType.NotFound);
        public static readonly Error NoteNotFound =
    new Error("Note.NoteNotFound", "Note not found.", ErrorType.NotFound);
    }
}