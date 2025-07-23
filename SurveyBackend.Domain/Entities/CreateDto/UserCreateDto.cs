namespace SurveyBackend.Domain.Entities.CreateDto
{
    public class UserCreateDTO
    {
        public string full_name { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string role { get; set; } = "rater"; // admin, manager, rater
        public int department_id { get; set; }
        public string? password { get; set; } // Optional, only needed for admin login
    }
}