using System.ComponentModel.DataAnnotations;

namespace Lazuli.Data;

public class SignupModel
{
    [Required]
    public string? Login { get; set; }

    [Required]
    public string? Password { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int BoundToUserId { get; set; }
}
