using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StudyFlow.Domain.Entities
{
    [Table("refresh_tokens")]
    [Index(nameof(TokenHash), IsUnique = true)]
    public class RefreshToken
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("token_hash")]
        [StringLength(200)]
        [Unicode(false)]
        public string TokenHash { get; set; } = null!;

        [Column("expires_at", TypeName = "datetime")]
        public DateTime ExpiresAt { get; set; }

        [Column("created_at", TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }

        [Column("revoked_at", TypeName = "datetime")]
        public DateTime? RevokedAt { get; set; }

        [Column("replaced_by_token_hash")]
        [StringLength(200)]
        [Unicode(false)]
        public string? ReplacedByTokenHash { get; set; }

        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

        public bool IsRevoked => RevokedAt != null;

        public bool IsActive => !IsExpired && !IsRevoked;

        public User User { get; set; } = null!;
    }
}
