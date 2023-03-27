using System.ComponentModel.DataAnnotations;
using LazuliLibrary.Utils;


namespace Lazuli.Data.Database;

public class User
{
    public int Id { get; set; }

    [Required]
    public string? Login { get; set; }

    [Required]
    public byte[]? Password { get; set; }

    /// <summary>
    /// Which user id from jsonplaceholder api to bind this user to
    /// </summary>
    [Required]
    public int BoundToUserId { get; set; }

    public User() { }

    public User(string login, string password, int boundToUserId)
    {
        Login = login;
        Password = CipherUtility.Encrypt(password, login);
        BoundToUserId = boundToUserId;
    }
}
