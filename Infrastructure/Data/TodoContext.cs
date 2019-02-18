using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
        : base(options)
        { }

        public DbSet<TodoItem> TodoItems { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Audit> Audits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItem>().HasOne(c => c.Category);

            modelBuilder
                .Entity<TodoItem>()
                .HasQueryFilter(t => !t.IsDeleted);

            modelBuilder
                .Entity<Category>()
                .HasQueryFilter(c => !c.IsDeleted);
        }
    }
}
