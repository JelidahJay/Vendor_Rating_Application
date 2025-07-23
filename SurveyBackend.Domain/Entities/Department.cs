using System.ComponentModel.DataAnnotations;

namespace SurveyBackend.Domain.Entities;

public class Department
{
    [Key]
    public int department_id { get; set; }

    public string name { get; set; } = string.Empty;

    public ICollection<User>? users { get; set; }
}
