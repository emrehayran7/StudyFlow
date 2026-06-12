using System.ComponentModel.DataAnnotations;

namespace StudyFlow.Core.Commands.Auth.Register.Request
{
    public class RegisterDto
    {
        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        public string? EducationLevel { get; set; }
    }
}
