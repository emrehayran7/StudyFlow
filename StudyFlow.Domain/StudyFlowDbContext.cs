using Microsoft.EntityFrameworkCore;

namespace StudyFlow.Domain.Entities;

public partial class StudyFlowDbContext : DbContext
{
    public StudyFlowDbContext()
    {
    }

    public StudyFlowDbContext(DbContextOptions<StudyFlowDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AiRequest> AiRequests { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<FlashCard> FlashCards { get; set; }

    public virtual DbSet<Note> Notes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<StudySession> StudySessions { get; set; }

    public virtual DbSet<Topic> Topics { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserCourse> UserCourses { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    /*
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
                "Server=localhost;Database=StudyFlowDb;Trusted_Connection=True;TrustServerCertificate=True");
        }
    }
    */

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AiRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ai_reque__3213E83F09A8B84E");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Topic).WithMany(p => p.AiRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ai_reques__topic__6A30C649");

            entity.HasOne(d => d.User).WithMany(p => p.AiRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ai_reques__user___693CA210");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("refresh_tokens");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.TokenHash)
                .IsRequired();

            entity.Property(x => x.CreatedAt)
                .HasColumnType("datetime");

            entity.Property(x => x.ExpiresAt)
                .HasColumnType("datetime");

            entity.Property(x => x.RevokedAt)
                .HasColumnType("datetime");

            entity.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__courses__3213E83FC81B4B43");
        });

        modelBuilder.Entity<FlashCard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__flash_ca__3213E83FCD7F16FA");

            entity.HasOne(d => d.Topic)
                .WithMany(p => p.FlashCards)
                .HasConstraintName("FK__flash_car__topic__5812160E");
        });

        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__notes__3213E83FF2FBC398");

            entity.HasOne(d => d.Topic)
                .WithMany(p => p.Notes)
                .HasConstraintName("FK__notes__topic_id__5535A963");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__roles__3213E83F7D4D6A5F");
        });

        modelBuilder.Entity<StudySession>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__study_se__3213E83F71EA75AB");

            entity.HasOne(d => d.Topic)
                .WithMany(p => p.StudySessions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__study_ses__topic__656C112C");

            entity.HasOne(d => d.User)
                .WithMany(p => p.StudySessions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__study_ses__user___6477ECF3");
        });

        modelBuilder.Entity<Topic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__topics__3213E83F9D7624E8");

            entity.HasOne(d => d.Course)
                .WithMany(p => p.Topics)
                .HasConstraintName("FK__topics__course_i__52593CB8");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83FEA9B3A5E");
        });

        modelBuilder.Entity<UserCourse>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__user_cou__3213E83FD1002128");

            entity.HasOne(d => d.Course)
                .WithMany(p => p.UserCourses)
                .HasConstraintName("FK__user_cour__cours__5CD6CB2B");

            entity.HasOne(d => d.User)
                .WithMany(p => p.UserCourses)
                .HasConstraintName("FK__user_cour__user___5BE2A6F2");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__user_rol__3213E83FBC8F1FCD");

            entity.HasOne(d => d.Role)
                .WithMany(p => p.UserRoles)
                .HasConstraintName("FK__user_role__role___619B8048");

            entity.HasOne(d => d.User)
                .WithMany(p => p.UserRoles)
                .HasConstraintName("FK__user_role__user___60A75C0F");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}