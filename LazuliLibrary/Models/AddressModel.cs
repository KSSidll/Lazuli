namespace LazuliLibrary.Models;

public class AddressModel
{
    public string? Street { get; set; }
    public string? Suite { get; set; }
    public string? City { get; set; }
    public string? Zipcode { get; set; }
    public GeoLocationModel? GeoLocation { get; set; }

    public override string ToString()
    {
        return $"{Zipcode} {City} {Street}";
    }
}
