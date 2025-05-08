using Hackathon_Application_Database.Models;
using Microsoft.EntityFrameworkCore;


namespace Hackathon_Application_Database.DatabaseContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Matter> Matters { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<NotificationLog> NotificationLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Matter>()
                .HasKey(m => m.MatterId);

            modelBuilder.Entity<Matter>()
            .Property(m => m.Status)
                .IsRequired();

            modelBuilder.Entity<Matter>()
                .Property(m => m.Title)
            .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<Matter>()
                .Property(m => m.ClientEmail)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<Document>()
                .HasKey(d => d.DocumentId);

            modelBuilder.Entity<Document>()
                .Property(d => d.Status)
                .IsRequired();

            modelBuilder.Entity<Document>()
                .Property(d => d.FileName)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<Document>()
                .Property(d => d.ContentType)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<NotificationLog>()
                .HasKey(n => n.NotificationId);

            modelBuilder.Entity<NotificationLog>()
                .Property(n => n.Status)
                .IsRequired();

            modelBuilder.Entity<NotificationLog>()
                .Property(n => n.EmailTo)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<NotificationLog>()
                .Property(n => n.Subject)
                .IsRequired()
                .HasMaxLength(255);
        }
    }
}
