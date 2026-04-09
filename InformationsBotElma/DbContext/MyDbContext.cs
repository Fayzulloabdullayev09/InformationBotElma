using InformationsBotElma.Configuration;
using InformationsBotElma.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InformationsBotElma.Data;

public class MyDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public DbSet<UserModel> Users => Set<UserModel>();
    public DbSet<TaskModel> Tasks => Set<TaskModel>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            return;
        }

        // Database connection string AppConfiguration.Database.ConnectionString ichiga yoziladi.
        optionsBuilder.UseNpgsql(AppConfiguration.Database.ConnectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserModel>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(user => user.UserId);

            entity.Property(user => user.UserId)
                .HasColumnName("user_id")
                .ValueGeneratedNever();

            entity.Property(user => user.UserName)
                .HasColumnName("user_name")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(user => user.Role)
                .HasColumnName("role")
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(user => user.RegistrationDateTime)
                .HasColumnName("registration_date_time")
                .IsRequired();
        });

        modelBuilder.Entity<TaskModel>(entity =>
        {
            entity.ToTable("tasks");
            entity.HasKey(task => task.TaskId);

            entity.Property(task => task.TaskId)
                .HasColumnName("task_id")
                .ValueGeneratedNever();

            entity.Property(task => task.TaskName)
                .HasColumnName("task_name")
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(task => task.Description)
                .HasColumnName("task_description")
                .HasMaxLength(1000)
                .IsRequired();

            entity.Property(task => task.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            entity.Property(task => task.DeadLine)
                .HasColumnName("deadline")
                .IsRequired();

            entity.Property(task => task.LastChangedAt)
                .HasColumnName("last_changed_at")
                .IsRequired();

            entity.Property(task => task.LastChangedByUserName)
                .HasColumnName("last_changed_by_user_name")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(task => task.TaskStatus)
                .HasColumnName("task_status")
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();
        });
    }
}
