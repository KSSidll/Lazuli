using System.ComponentModel.DataAnnotations;

public class SignupModel
{
    [Required]
    public string? Login { get; set; }

    [Required]
    public string? Password { get; set; }

    [Required]
    public int BoundToUserId { get; set; }
}
