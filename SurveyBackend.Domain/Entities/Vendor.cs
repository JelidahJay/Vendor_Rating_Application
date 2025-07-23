using System.ComponentModel.DataAnnotations;

namespace SurveyBackend.Domain.Entities;

public class Vendor
{
    [Key]
    public int vendor_id { get; set; }

    public string name { get; set; } = string.Empty;
    public string product_service { get; set; } = string.Empty;

    public ICollection<Survey>? surveys { get; set; }
}
