using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StudyFlow.Domain.Entities;

[Table("user_courses")]
[Index("UserId", "CourseId", Name = "UQ__user_cou__414FD874E9F19E97", IsUnique = true)]
public partial class UserCourse
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("course_id")]
    public int CourseId { get; set; }

    [ForeignKey("CourseId")]
    [InverseProperty("UserCourses")]
    public virtual Course Course { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("UserCourses")]
    public virtual User User { get; set; } = null!;


}
