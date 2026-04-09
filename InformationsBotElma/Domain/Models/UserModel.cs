using System.ComponentModel.DataAnnotations.Schema;
using InformationsBotElma.Domain.Enums;

namespace InformationsBotElma.Domain.Models;

[Table("users")]
public class UserModel
{
    [Column("user_id")]
    public long UserId { get; set; }

    [Column("user_name")]
    public required string UserName { get; set; }

    [Column("role")]
    public UserRole Role { get; set; }

    [Column("registration_date_time")]
    public DateTime RegistrationDateTime { get; set; }
}
