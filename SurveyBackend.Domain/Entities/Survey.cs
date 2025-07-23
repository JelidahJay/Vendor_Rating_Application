using System.ComponentModel.DataAnnotations;

namespace SurveyBackend.Domain.Entities;

public class Survey
{
    [Key]
    public int survey_id { get; set; }

    public int vendor_id { get; set; }
    public Vendor? vendor { get; set; }

    public int user_id { get; set; } // Rater
    public User? user { get; set; }

    public int invited_by_user_id { get; set; } // Admin/Manager
    public User? invited_by { get; set; }

    public string token { get; set; } = string.Empty; // Secure survey access
    public DateTime valid_until { get; set; }
    public DateTime? submitted_at { get; set; }

    public string status { get; set; } = "pending"; // pending, completed

    public ICollection<SurveyResponse>? responses { get; set; }
}
