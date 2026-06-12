using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StudyFlow.Domain.Entities;

[Table("courses")]
public partial class Course
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("title")]
    [StringLength(255)]
    [Unicode(false)]
    public string Title { get; set; } = null!;

    [Column("description")]
    [Unicode(false)]
    public string? Description { get; set; }
    /// <summary>
    /// Timestamp indicating when the topic was created.
    /// Stored in UTC to ensure consistency across time zones.
    /// </summary>

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "datetime")]

    /// <summary>
    /// Timestamp indicating when the topic was updated.
    /// Stored in UTC to ensure consistency across time zones.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Identifier of the user who created the course.
    /// </summary>

    [Column("created_by")]
    [StringLength(100)]
    [Unicode(false)]
    public string? CreatedBy { get; set; }

    /// <summary>
    /// Identifier of the user who upadted the course.
    /// </summary>
    [Column("updated_by")]
    [StringLength(100)]
    [Unicode(false)]
    public string? UpdatedBy { get; set; }

    [InverseProperty("Course")]
    public virtual ICollection<Topic> Topics { get; set; } = new List<Topic>();

    [InverseProperty("Course")]
    public virtual ICollection<UserCourse> UserCourses { get; set; } = new List<UserCourse>();
}
