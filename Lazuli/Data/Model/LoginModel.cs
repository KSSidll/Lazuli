using System.ComponentModel.DataAnnotations;

public class LoginModel
{
    [Required]
    [MinLength(1, ErrorMessage = "pwd err")]
    public string? Login { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "pwd err")]
    public string? Password { get; set; }
}
