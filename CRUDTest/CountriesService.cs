using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using Entities;
using Xunit;

namespace CRUDTest;
public class CountriesServiceTest
{
    private readonly ICountriesService _countriesService;

    //constructor
    public CountriesServiceTest()
    {
        _countriesService = new CountriesService(false);
    }

    #region AddCountry

    [Fact]
    public void AddCountry_NullCountry()
    {
        //Arrange
        CountryAddRequest? request = null;
        //Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            //Act
            _countriesService.AddCountry(request);
        });
    }

    [Fact]
    public void AddCountry_CountryNameIsNull()
    {
        CountryAddRequest? request = new CountryAddRequest() { CountryName = null };
        Assert.Throws<ArgumentException>(() =>
        {
            _countriesService.AddCountry(request);
        });
    }

    [Fact]
    public void AddCountry_DuplicateCountryName()
    {
        CountryAddRequest? request1 = new CountryAddRequest() { CountryName = "Canada" };
        CountryAddRequest? request2 = new CountryAddRequest() { CountryName = "Canada" };

        Assert.Throws<ArgumentException>(() =>
        {
            _countriesService.AddCountry(request1);
            _countriesService.AddCountry(request2);
        });
    }

    [Fact]
    public void AddCountry_ProperCountry()
    {
        CountryAddRequest? request = new CountryAddRequest() { CountryName = "Japan" };

        CountryResponse response = _countriesService.AddCountry(request);
        Assert.True(response.CountryID != Guid.Empty);
        Assert.Contains(response, _countriesService.GetAllCountries());
    }

    //When CountryAddRequest is null, ArgumentNullException
    //CountryName is null, throw ArgumentException
    //CountryName is duplicate, throw ArgumentException
    //Proper Country name, insert the country to existing listing of countries

    #endregion

    #region GetAllCountries

    [Fact]
    //empty list by default
    public void GetAllCountries_EmptyList()
    {
        //Act
        List<CountryResponse> actual_countries_response_list = _countriesService.GetAllCountries();

        //Assert
        Assert.Empty(actual_countries_response_list);
    }

    [Fact]
    public void GetAllCountries_ReturnsAddedCountries()
    {
        //Arrange
        List<CountryAddRequest> countryAddRequests = new() {
            new CountryAddRequest() {CountryName = "Canada"},
            new CountryAddRequest() {CountryName = "America"},
            new CountryAddRequest() {CountryName = "Japan"}
        };

        //Act
        List<CountryResponse> countryResponses = new();
        foreach (CountryAddRequest country in countryAddRequests)
        {
            countryResponses.Add(
            _countriesService.AddCountry(country));
        }

        List<CountryResponse> actualCountryResponseList = _countriesService.GetAllCountries();

        foreach (CountryResponse expected_country in countryResponses)
        {
            Assert.Contains(expected_country, actualCountryResponseList);
        }
    }

    #endregion

    #region GetCountryByCountryID

    [Fact]
    public void GetCountryByCountryID_NullCountryId_ReturnsNull()
    {
        //Arrange
        Guid? countryID = null;

        //Act
        CountryResponse? countryResponse = _countriesService.GetCountryByCountryID(countryID);

        //Assert
        Assert.Null(countryResponse);
    }

    [Fact]
    public void GetCountryByCountryID_ValidId_ProperReturn()
    {
        //Arrange
        CountryAddRequest? country_add_request = new CountryAddRequest() { CountryName = "China" };
        CountryResponse countryResponse = _countriesService.AddCountry(country_add_request);
        //Act
        CountryResponse actulCountryResponse = _countriesService.GetCountryByCountryID(countryResponse.CountryID)!;
        //Assert
        Assert.Equal(countryResponse, actulCountryResponse);
    }
    #endregion
}