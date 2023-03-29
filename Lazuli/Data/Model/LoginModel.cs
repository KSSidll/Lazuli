using System.ComponentModel.DataAnnotations;

namespace Lazuli.Data;

public class LoginModel
{
    [Required]
    public string? Login { get; set; }

    [Required]
    public string? Password { get; set; }
}
