using ServiceContracts.DTO;

namespace ServiceContracts;

public interface ICountriesService
{
    //add a country object to the list of countries
    //param=country object to add
    //returns the country object after adding it

    CountryResponse AddCountry(CountryAddRequest? countryAddRequest);

    /// <summary>
    /// Returns all countries from the list of countries
    /// </summary>
    /// <returns>All countries from te list as list of countryresponses</returns>
    List<CountryResponse> GetAllCountries();

    /// <summary>
    /// Returns a Country object based on the provided country id
    /// </summary>
    /// <param name="countryID">CountrID (guid) to search</param>
    /// <returns>Matching country as CountryResponse object</returns>
    CountryResponse? GetCountryByCountryID(Guid? CountryID);
}
