using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StudyFlow.Domain.Entities;

[Table("flash_cards")]
public partial class FlashCard
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("topic_id")]
    public int TopicId { get; set; }

    [Column("question")]
    [Unicode(false)]
    public string Question { get; set; } = null!;

    [Column("answer")]
    [Unicode(false)]
    public string Answer { get; set; } = null!;

    /// <summary>
    /// Optional hint to help recall the answer.
    /// </summary>
    [Column("hint")]
    [StringLength(50)]
    [Unicode(false)]
    public string Hint { get; set; } = null!;
    /// <summary>
    /// Difficulty level of the flashcard.
    /// </summary>
    [Column("difficulty_level")]
    public int DifficultyLevel { get; set; }

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
    /// Identifier of the user who created the flashcard.
    /// </summary>
    [Column("created_by")]
    [StringLength(100)]
    [Unicode(false)]
    public string? CreatedBy { get; set; }
    /// <summary>
    /// Identifier of the user who updated the flashcard.
    /// </summary>
    [Column("updated_by")]
    [StringLength(100)]
    [Unicode(false)]
    public string? UpdatedBy { get; set; }

    [ForeignKey("TopicId")]
    [InverseProperty("FlashCards")]
    public virtual Topic Topic { get; set; } = null!;
}
