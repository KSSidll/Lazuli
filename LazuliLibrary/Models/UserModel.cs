namespace LazuliLibrary.Models;

public class UserModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public AddressModel? Address { get; set; }
    public string? Phone { get; set; }
    public string? Website { get; set; }
    public CompanyModel? Company { get; set; }

    public override string ToString()
    {
        return $"{Id} {Name} {Username}";
    }
}
