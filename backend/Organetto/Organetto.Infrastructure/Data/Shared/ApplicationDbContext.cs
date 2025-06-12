using Microsoft.EntityFrameworkCore;
using Organetto.Core.Boards.Cards.Data;
using Organetto.Core.Boards.Data;
using Organetto.Core.Shared.Databases;
using Organetto.Core.Shared.Databases.Transactions;
using Organetto.Core.Users.Data;
using Organetto.Infrastructure.Data.Shared.Transactions;

namespace Organetto.Infrastructure.Data.Shared
{
    /// <summary>
    /// The EF Core DbContext, reflecting the ER diagram in code.
    /// </summary>
    public class ApplicationDbContext : DbContext, IUnitOfWork
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet properties for each entity (наборы сущностей)
        public DbSet<User> Users { get; set; }
        public DbSet<Board> Boards { get; set; }
        public DbSet<BoardMember> BoardMembers { get; set; }
        public DbSet<BoardList> BoardLists { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<DueDate> DueDates { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public Task<IDatabaseTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IDatabaseTransaction>(new EntityFrameworkDatabaseTransaction(this));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // USERS
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");                             // Table name mapping (сопоставление имени таблицы)
                entity.HasKey(u => u.Id);                            // Primary key (первичный ключ)
                entity.Property(u => u.FirebaseUid)
                      .IsRequired()
                      .HasMaxLength(36);
                entity.HasIndex(u => u.FirebaseUid)
                      .IsUnique();                                   // Unique index on FirebaseUid (уникальный индекс)
                entity.Property(u => u.Email)
                      .IsRequired()
                      .HasMaxLength(256);
                entity.Property(u => u.Name)
                      .IsRequired()
                      .HasMaxLength(128);
                entity.Property(u => u.CreatedAt)
                      .IsRequired()
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            // BOARDS
            modelBuilder.Entity<Board>(entity =>
            {
                entity.ToTable("board");                            // Table name
                entity.HasKey(b => b.Id);
                entity.Property(b => b.Title)
                      .IsRequired()
                      .HasMaxLength(256);
                entity.Property(b => b.Description)
                      .HasColumnType("text");
                entity.Property(b => b.CreatedAt)
                      .IsRequired()
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(b => b.UpdatedAt)
                      .IsRequired()
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(b => b.IsArchived)
                      .IsRequired()
                      .HasDefaultValue(false);

                // Relationship: Board.Owner -> User
                entity.HasOne(b => b.Owner)
                      .WithMany(u => u.OwnedBoards)
                      .HasForeignKey(b => b.OwnerId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // BOARD_MEMBERS
            modelBuilder.Entity<BoardMember>(entity =>
            {
                entity.ToTable("board_member");
                entity.HasKey(bm => bm.Id);
                entity.Property(bm => bm.Role)
                      .IsRequired()
                      .HasMaxLength(64);

                // Relationship: BoardMember.Board -> Board
                entity.HasOne(bm => bm.Board)
                      .WithMany(b => b.Members)
                      .HasForeignKey(bm => bm.BoardId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Relationship: BoardMember.User -> User
                entity.HasOne(bm => bm.User)
                      .WithMany(u => u.BoardMemberships)
                      .HasForeignKey(bm => bm.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Composite unique constraint: one user can be member of a board only once
                entity.HasIndex(bm => new { bm.BoardId, bm.UserId })
                      .IsUnique();
            });

            // LISTS (BoardList)
            modelBuilder.Entity<BoardList>(entity =>
            {
                entity.ToTable("board_list");                             // Using “Lists” as table name
                entity.HasKey(l => l.Id);
                entity.Property(l => l.Title)
                      .IsRequired()
                      .HasMaxLength(256);
                entity.Property(l => l.Position)
                      .IsRequired();

                // Relationship: BoardList.Board -> Board
                entity.HasOne(l => l.Board)
                      .WithMany(b => b.Lists)
                      .HasForeignKey(l => l.BoardId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Composite unique constraint: each board's list positions should be unique
                entity.HasIndex(l => new { l.BoardId, l.Position })
                      .IsUnique();
            });

            // CARDS
            modelBuilder.Entity<Card>(entity =>
            {
                entity.ToTable("card");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Title)
                      .IsRequired()
                      .HasMaxLength(256);
                entity.Property(c => c.Description)
                      .HasColumnType("text");
                entity.Property(c => c.Position)
                      .IsRequired();
                entity.Property(c => c.CreatedAt)
                      .IsRequired()
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(c => c.UpdatedAt)
                      .IsRequired()
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");

                // Relationship: Card.BoardList -> BoardList
                entity.HasOne(c => c.BoardList)
                      .WithMany(l => l.Cards)
                      .HasForeignKey(c => c.BoardListId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Composite unique constraint: each list’s card positions unique
                entity.HasIndex(c => new { c.BoardListId, c.Position })
                      .IsUnique();
            });

            // COMMENTS
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("comment");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Body)
                      .IsRequired()
                      .HasColumnType("text");
                entity.Property(c => c.CreatedAt)
                      .IsRequired()
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");

                // Relationship: Comment.Card -> Card
                entity.HasOne(c => c.Card)
                      .WithMany(cd => cd.Comments)
                      .HasForeignKey(c => c.CardId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Relationship: Comment.Author -> User
                entity.HasOne(c => c.Author)
                      .WithMany(u => u.Comments)
                      .HasForeignKey(c => c.AuthorId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ATTACHMENTS
            modelBuilder.Entity<Attachment>(entity =>
            {
                entity.ToTable("attachment");
                entity.HasKey(a => a.Id);
                entity.Property(a => a.FileUrl)
                      .IsRequired()
                      .HasMaxLength(1024);
                entity.Property(a => a.Filename)
                      .IsRequired()
                      .HasMaxLength(512);
                entity.Property(a => a.UploadedAt)
                      .IsRequired()
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");

                // Relationship: Attachment.Card -> Card
                entity.HasOne(a => a.Card)
                      .WithMany(c => c.Attachments)
                      .HasForeignKey(a => a.CardId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Relationship: Attachment.Uploader -> User
                entity.HasOne(a => a.Uploader)
                      .WithMany(u => u.Attachments)
                      .HasForeignKey(a => a.UploaderId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // DUE_DATES
            modelBuilder.Entity<DueDate>(entity =>
            {
                entity.ToTable("due_date");
                entity.HasKey(d => d.Id);
                entity.Property(d => d.DueAt)
                      .IsRequired();
                entity.Property(d => d.IsComplete)
                      .IsRequired()
                      .HasDefaultValue(false);

                // Relationship: DueDate.Card -> Card
                entity.HasOne(d => d.Card)
                      .WithMany(c => c.DueDates)
                      .HasForeignKey(d => d.CardId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Enforce one due date per card if desired:
                // entity.HasIndex(d => d.CardId).IsUnique();
            });

            // NOTIFICATIONS
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("notification");
                entity.HasKey(n => n.Id);
                entity.Property(n => n.Message)
                      .IsRequired()
                      .HasColumnType("text");
                entity.Property(n => n.SentAt)
                      .IsRequired()
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(n => n.IsRead)
                      .IsRequired()
                      .HasDefaultValue(false);

                // Relationship: Notification.User -> User
                entity.HasOne(n => n.User)
                      .WithMany(u => u.Notifications)
                      .HasForeignKey(n => n.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
