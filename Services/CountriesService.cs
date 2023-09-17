using ServiceContracts;
using ServiceContracts.DTO;
using Entities;

namespace Services;

public class CountriesService : ICountriesService
{
    private readonly List<Country> _countries;

    //constructor initialization
    public CountriesService(bool initialize = true)
    {
        _countries = new List<Country>();
        if (initialize)
        {
            /*
            ddbc89e4-bd84-4ba0-b9cf-0208bd12f99d
            9a51e83c-997e-4f58-ba74-55137c596f54
            dc653733-44c0-4c52-a060-bf9d08daf378
            15582071-1713-499e-8ccf-493ceb961564
            c8e0ef40-1a41-4810-88c7-79b274b65ecb
            */
            _countries = new()
            {
                new Country() { CountryID = Guid.Parse("ddbc89e4-bd84-4ba0-b9cf-0208bd12f99d"), CountryName = "USA" },
                new Country() { CountryID = Guid.Parse("9a51e83c-997e-4f58-ba74-55137c596f54"), CountryName = "Canada" },
                new Country() { CountryID = Guid.Parse("dc653733-44c0-4c52-a060-bf9d08daf378"), CountryName = "India" },
                new Country() { CountryID = Guid.Parse("15582071-1713-499e-8ccf-493ceb961564"), CountryName = "Japan" },
                new Country() { CountryID = Guid.Parse("c8e0ef40-1a41-4810-88c7-79b274b65ecb"), CountryName = "South Korea" }
            };
        }
    }

    CountryResponse ICountriesService.AddCountry(CountryAddRequest? countryAddRequest)
    {
        //validation logic
        if (countryAddRequest == null)
        {
            throw new ArgumentNullException();
        }
        if (countryAddRequest.CountryName == null)
        {
            throw new ArgumentException();
        }
        if (_countries.Exists((country) => country.CountryName!.Equals(countryAddRequest.CountryName, StringComparison.InvariantCultureIgnoreCase)))
        {
            throw new ArgumentException();
        }

        //convert request to country
        Country country = countryAddRequest.ToCountry();
        //generate country id
        country.CountryID = Guid.NewGuid();
        //add country to the list
        _countries.Add(country);
        //return the country response: convert country to a countryresponse object
        return country.ToCountryResponse(); //this is from the countryresponse class, the static class used to convert the country into the response object
    }

    public List<CountryResponse> GetAllCountries()
    {
        // var responses = _countries.Select(country => country.ToCountryResponse());
        // List<CountryResponse> countryResponses = new();
        // foreach (var response in responses)
        // {
        //     countryResponses.Add(response);
        // }
        // return countryResponses;
        return _countries.Select(country => country.ToCountryResponse()).ToList();
    }

    public CountryResponse? GetCountryByCountryID(Guid? CountryID)
    {
        if (CountryID == null) return null;

        Country? country = _countries.FirstOrDefault(temp => temp.CountryID == CountryID);

        if (country == null) return null;

        return country.ToCountryResponse();

    }
}
