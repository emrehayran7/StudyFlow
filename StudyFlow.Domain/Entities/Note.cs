using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StudyFlow.Domain.Entities;

[Table("notes")]
public partial class Note
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("topic_id")]
    public int TopicId { get; set; }

    [Column("title")]
    [StringLength(255)]
    [Unicode(false)]
    public string Title { get; set; } = null!;

    [Column("content")]
    [Unicode(false)]
    public string? Content { get; set; }

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

    [ForeignKey("TopicId")]
    [InverseProperty("Notes")]
    public virtual Topic Topic { get; set; } = null!;
}
