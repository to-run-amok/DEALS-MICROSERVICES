using System.ComponentModel.DataAnnotations;

public class LoginDto
{   
    [Required]
    [EmailAddress]
    public string Email {get; set;} = string.Empty;
    
    public string Password {get; set;} = string.Empty;
}