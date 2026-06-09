using System.ComponentModel.DataAnnotations;

public class LoginDto
{
    [Required]
    [MinLength(3)]
    public string Email {get; set;}

    [Required]
    [MinLength(3)]
    public string Password {get; set; }
}

public class RegisterDto
{
    [Required]
    [MinLength(3)]
    public string Name {get; set;}

    [Required]
    [EmailAddress]
    public string Email {get; set;}

    [Required]
    [MinLength(5)]
    public string Password {get; set; }

    [Required]
    [MinLength(5)]
    public string ConfirmPassword {get; set; }
}