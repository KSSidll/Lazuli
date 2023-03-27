using System.ComponentModel.DataAnnotations;

public class LoginModel
{
    [Required]
    public string? Login { get; set; }

    [Required]
    public string? Password { get; set; }
}
