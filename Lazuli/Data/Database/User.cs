using System.ComponentModel.DataAnnotations;

namespace Lazuli.Data.Database;

public class User
{
    public int Id { get; set; }

    [Required]
    public string? Login { get; set; }

    [Required]
    public string? Password { get; set; }

    /// <summary>
    /// Which user id from jsonplaceholder api to bind this user to
    /// </summary>
    [Required]
    public int? BoundToUserId { get; set; }
}
