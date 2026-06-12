using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StudyFlow.Domain.Entities;

[Table("study_sessions")]
public partial class StudySession
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("topic_id")]
    public int TopicId { get; set; }

    /// <summary>
    /// Session start time stored in UTC.
    /// </summary>
    [Column("start_time", TypeName = "datetime")]
    public DateTime StartTime { get; set; }
    /// <summary>
    /// Session end time stored in UTC.
    /// </summary>
    [Column("end_time", TypeName = "datetime")]
    public DateTime EndTime { get; set; }
    /// <summary>
    /// Total duration of the session in minutes.
    /// </summary>
    [Column("duration_minutes")]
    public int? DurationMinutes { get; set; }
    /// <summary>
    /// Optional notes written during or after the session.
    /// </summary>
    [Column("session_notes")]
    [Unicode(false)]
    public string? SessionNotes { get; set; }

    [ForeignKey("TopicId")]
    [InverseProperty("StudySessions")]
    public virtual Topic Topic { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("StudySessions")]
    public virtual User User { get; set; } = null!;
}
