using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StudyFlow.Domain.Entities;

[Table("users")]
[Index("Email", Name = "UQ__users__AB6E6164995C198C", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("first_name")]
    [StringLength(50)]
    [Unicode(false)]
    public string FirstName { get; set; } = null!;

    [Column("last_name")]
    [StringLength(50)]
    [Unicode(false)]
    public string LastName { get; set; } = null!;

    [Column("email")]
    [StringLength(100)]
    [Unicode(false)]
    public string Email { get; set; } = null!;
    /// <summary>
    /// Hashed password stored as a byte64.
    /// </summary>
    [Column("password")]
    [MaxLength(64)]
    public byte[] Password { get; set; } = null!;

    [Column("education_level")]
    [StringLength(100)]
    [Unicode(false)]
    public string? EducationLevel { get; set; }
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
    /// Identifier of the user who created the user.
    /// </summary>
    [Column("created_by")]
    [StringLength(100)]
    [Unicode(false)]
    public string? CreatedBy { get; set; }
    /// <summary>
    /// Identifier of the user who updated the user.
    /// </summary>
    [Column("updated_by")]
    [StringLength(100)]
    [Unicode(false)]
    public string? UpdatedBy { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<AiRequest> AiRequests { get; set; } = new List<AiRequest>();

    [InverseProperty("User")]
    public virtual ICollection<StudySession> StudySessions { get; set; } = new List<StudySession>();

    [InverseProperty("User")]
    public virtual ICollection<UserCourse> UserCourses { get; set; } = new List<UserCourse>();

    [InverseProperty("User")]
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
