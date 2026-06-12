using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StudyFlow.Domain.Entities;

[Table("topics")]
public partial class Topic
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("course_id")]
    public int CourseId { get; set; }

    [Column("title")]
    [StringLength(255)]
    [Unicode(false)]
    public string Title { get; set; } = null!;

    [Column("description")]
    [Unicode(false)]
    public string? Description { get; set; }
    /// <summary>
    /// Current status of the topic (e.g., Pending, InProgress, Completed).
    /// Used for tracking study progress.
    /// </summary>
    [Column("status")]
    [StringLength(20)]
    [Unicode(false)]
    public string Status { get; set; } = null!;

    /// <summary>
    /// Indicates the priority level of the topic.
    /// Higher values may represent more important topics to study.
    /// </summary>

    [Column("priority_level")]
    public int? PriorityLevel { get; set; }

    /// <summary>
    /// Timestamp indicating when the topic was created.
    /// Stored in UTC to ensure consistency across time zones.
    /// </summary>

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    /// <summary>
    /// Timestamp indicating when the topic was updated.
    /// Stored in UTC to ensure consistency across time zones.
    /// </summary>

    [Column("updated_at", TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Identifier of the user who created the topic.
    /// </summary>
    [Column("created_by")]
    [StringLength(100)]
    [Unicode(false)]
    public string? CreatedBy { get; set; }

    /// <summary>
    /// Identifier of the user who updated the topic.
   
    /// </summary>
    [Column("updated_by")]
    [StringLength(100)]
    [Unicode(false)]
    public string? UpdatedBy { get; set; }

    [InverseProperty("Topic")]
    public virtual ICollection<AiRequest> AiRequests { get; set; } = new List<AiRequest>();

    [ForeignKey("CourseId")]
    [InverseProperty("Topics")]
    public virtual Course Course { get; set; } = null!;

    [InverseProperty("Topic")]
    public virtual ICollection<FlashCard> FlashCards { get; set; } = new List<FlashCard>();

    [InverseProperty("Topic")]
    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();

    [InverseProperty("Topic")]
    public virtual ICollection<StudySession> StudySessions { get; set; } = new List<StudySession>();
}
