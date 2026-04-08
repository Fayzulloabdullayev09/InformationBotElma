using System.ComponentModel.DataAnnotations.Schema;

namespace InformationsBotElma.Domen.Models;

[Table("users")]
public class UserModel
{
    [Column("user_id")]
    public long UserId { get; set; }
    [Column("user_name")]
    public string UserName { get; set; }
    [Column("registration_date_time")]
    public DateTime RegistrationDateTime { get; set; }
}