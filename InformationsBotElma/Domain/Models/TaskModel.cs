using System.ComponentModel.DataAnnotations.Schema;
using DomainTaskStatus = InformationsBotElma.Domain.Enums.TaskStatus;

namespace InformationsBotElma.Domain.Models;

[Table("tasks")]
public class TaskModel
{
    [Column("task_id")]
    public int TaskId { get; set; }

    [Column("task_name")]
    public required string TaskName { get; set; }

    [Column("task_description")]
    public required string Description { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("deadline")]
    public DateTime DeadLine { get; set; }

    [Column("last_changed_at")]
    public DateTime LastChangedAt { get; set; }

    [Column("last_changed_by_user_name")]
    public required string LastChangedByUserName { get; set; }

    public DomainTaskStatus TaskStatus { get; set; }
}
