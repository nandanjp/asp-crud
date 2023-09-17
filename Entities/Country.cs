namespace Entities;

public class Country
{
    // <summary>
    // Domain model for country (does not get exposed to the controller)
    // </summary>
    public Guid CountryID { get; set; }
    public string? CountryName { get; set; }
}
