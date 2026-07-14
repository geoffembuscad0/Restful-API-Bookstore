using Microsoft.EntityFrameworkCore;
using RestfulApiDemo.Models;

namespace RestfulApiDemo.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User entity configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(255);
                entity.Property(e => e.CreatedAt).HasDefaultValue(DateTime.Now);
                entity.Property(e => e.DeletedAt).IsRequired(false);
            });

            // Category entity configuration
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
                entity.Property(e => e.CreatedAt).HasDefaultValue(DateTime.Now);
                entity.Property(e => e.DeletedAt).IsRequired(false);
            });
        }
    }
}