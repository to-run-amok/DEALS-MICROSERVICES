using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

public class UserRegisterDto
{   
    [Required]
    [DefaultValue("John Doe")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [DefaultValue("john@example.com")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).+$",ErrorMessage = "Password must contain uppercase, lowercase, digit and special character.")]
    [DefaultValue("John@2049")]
    public string Password { get; set; } = string.Empty;

    [Phone]
    public string Phone { get; set; } = string.Empty;

    [Required]
    public UserRole Role { get; set; }
    public string? AccountNumber { get; set; }

    public string? BankName { get; set; }

    public string? IFSCCode { get; set; }
}