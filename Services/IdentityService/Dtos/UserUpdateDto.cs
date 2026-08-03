using System.ComponentModel.DataAnnotations;

public class UserUpdateDto
{   
    [Required]
    public string? Name {get; set;}

    [Required]
    public string? Phone {get; set;}
    public string? AccountNumber { get; set; }
    public string? BankName { get; set; }
    public string? IFSCCode { get; set; }
}