using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
        : base(options)
        { }

        public DbSet<TodoItem> TodoItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TodoItem>(ConfigureTodoItem);
        }

        private void ConfigureTodoItem(EntityTypeBuilder<TodoItem> builder)
        {
            builder
                .HasKey(t => t.Id)
                .HasName("PrimaryKey_TodoItemId");

            builder.Property(t => t.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(t => t.Description)
                .HasMaxLength(500);

            builder.Property(t => t.Priority)
                .IsRequired();

            builder.Property(t => t.Responsible)
                .HasMaxLength(50);

            builder.Property(t => t.Created)
                .IsRequired();

            builder.Property(t => t.Creator)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(t => t.Modified)
                .IsRequired();

            builder.Property(t => t.Modifier)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(t => t.IsDeleted)
                .HasDefaultValue(false)
                .IsRequired();
        }
    }
}
