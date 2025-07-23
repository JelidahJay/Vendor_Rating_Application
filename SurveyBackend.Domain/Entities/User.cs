using System.ComponentModel.DataAnnotations;

namespace SurveyBackend.Domain.Entities;
public class User
{
    [Key]
    public int user_id { get; set; }

    public string full_name { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
    public string role { get; set; } = "rater"; // "admin", "manager", "rater"

    public int department_id { get; set; }
    public Department? department { get; set; }

    public string? password { get; set; } // only for admins

    public ICollection<Survey>? surveys_rated { get; set; }
    public ICollection<Survey>? surveys_invited { get; set; }
}
