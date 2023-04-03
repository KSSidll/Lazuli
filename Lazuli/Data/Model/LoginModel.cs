using System.ComponentModel.DataAnnotations;

namespace Lazuli.Data.Models;

public class LoginModel
{
    [Required]
    [MinLength(1)]
    public string? Login { get; set; }

    [Required]
    [MinLength(1)]
    public string? Password { get; set; }
}
