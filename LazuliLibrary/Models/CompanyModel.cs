namespace LazuliLibrary.Models;

public class CompanyModel
{
    public string? Name { get; set; }
    public string? CatchPhrase { get; set; }
    public string? BusinessStrategy { get; set; }

    public override string ToString()
    {
        return $"{Name} {CatchPhrase} {BusinessStrategy}";
    }
}
