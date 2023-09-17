using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;

namespace Services;
public class PersonsService : IPersonsService
{
    private readonly List<Person> _persons;
    private readonly ICountriesService _countriesService;

    public PersonsService(bool initialize = true)
    {
        _persons = new List<Person>();
        if (!initialize)
        {
            _countriesService = new CountriesService(false);
        }
        else
        {
            _countriesService = new CountriesService();
        }

        if (initialize)
        {
            _persons = new()
            {
                new() {
                    PersonName = "Sue",
                    Email = "sderuggiero0@wiley.com",
                    DateOfBirth = DateTime.Parse("8/7/2002"),
                    Gender = "Female",
                    Address = "1 Northland Avenue",
                    ReceiveNewsLetters = false,
                    PersonID = Guid.Parse("95226123-74d4-4f5f-b3f7-15a2be71aa1a"),
                    CountryID = Guid.Parse("ddbc89e4-bd84-4ba0-b9cf-0208bd12f99d")
                },
                new() {
                    PersonName = "Jillane",
                    Email = "jkite1@oakley.com",
                    DateOfBirth = DateTime.Parse("11/20/2001"),
                    Gender = "Female",
                    Address = "00181 Drewry Park",
                    ReceiveNewsLetters = true,
                    PersonID = Guid.Parse("a2e96acd-0c9b-40aa-9a86-ccf2ea87f48e"),
                    CountryID = Guid.Parse("9a51e83c-997e-4f58-ba74-55137c596f54")
                },
                new() {
                    PersonName = "Orella",
                    Email = "omatzel2@npr.org",
                    DateOfBirth = DateTime.Parse("9/19/2005"),
                    Gender = "Female",
                    Address = "3 Ludington Point",
                    ReceiveNewsLetters = false,
                    PersonID = Guid.Parse("808f328d-94c5-49b5-8ce1-77e749f07488"),
                    CountryID = Guid.Parse("dc653733-44c0-4c52-a060-bf9d08daf378")
                },
                new() {
                    PersonName = "Stavro",
                    Email = "smontague3@mac.com",
                    DateOfBirth = DateTime.Parse("11/13/2007"),
                    Gender = "Male",
                    Address = "17 Sutherland Circle",
                    ReceiveNewsLetters = false,
                    PersonID = Guid.Parse("7f42fb89-4675-4b4c-895b-4aaa1f398f67"),
                    CountryID = Guid.Parse("15582071-1713-499e-8ccf-493ceb961564")
                },
                new() {
                    PersonName = "Kaspar",
                    Email = "kleisman4@hhs.gov",
                    DateOfBirth = DateTime.Parse("10/30/2006"),
                    Gender = "Male",
                    Address = "5726 Burning Wood Junction",
                    ReceiveNewsLetters = true,
                    PersonID = Guid.Parse("77ac3005-6070-49b0-88f0-87fa26e87be2"),
                    CountryID = Guid.Parse("c8e0ef40-1a41-4810-88c7-79b274b65ecb")
                },
                new() {
                    PersonName = "Terese",
                    Email = "tmanns5@toplist.cz",
                    DateOfBirth = DateTime.Parse("10/26/2002"),
                    Gender = "Female",
                    Address = "0421 Steensland Way",
                    ReceiveNewsLetters = false,
                    PersonID = Guid.Parse("3422a64a-695d-4fe5-a037-8ae1c5755be7"),
                    CountryID = Guid.Parse("c8e0ef40-1a41-4810-88c7-79b274b65ecb")
                },
                new() {
                    PersonName = "Ambur",
                    Email = "adaverin6@craigslist.org",
                    DateOfBirth = DateTime.Parse("1/1/2009"),
                    Gender = "Female",
                    Address = "1279 Moulton Drive",
                    ReceiveNewsLetters = false,
                    PersonID = Guid.Parse("b84dd74f-b5ab-4725-a514-a8f9cd487191"),
                    CountryID = Guid.Parse("c8e0ef40-1a41-4810-88c7-79b274b65ecb")
                },
                new() {
                    PersonName = "Clareta",
                    Email = "cpearce7@bigcartel.com",
                    DateOfBirth = DateTime.Parse("5/24/2010"),
                    Gender = "Female",
                    Address = "4160 Gale Street",
                    ReceiveNewsLetters = true,
                    PersonID = Guid.Parse("0cf61174-acd8-420c-a0eb-fcbec02ece61"),
                    CountryID = Guid.Parse("c8e0ef40-1a41-4810-88c7-79b274b65ecb")
                },
                new() {
                    PersonName = "Diarmid",
                    Email = "dmacduff8@chron.com",
                    DateOfBirth = DateTime.Parse("3/22/2004"),
                    Gender = "Male",
                    Address = "08 Merchant Avenue",
                    ReceiveNewsLetters = false,
                    PersonID = Guid.Parse("01d65c49-52e2-44e0-a117-f4260f3391f1"),
                    CountryID = Guid.Parse("c8e0ef40-1a41-4810-88c7-79b274b65ecb")
                },
                new() {
                    PersonName = "Myer",
                    Email = "msaunderson9@usatoday.com",
                    DateOfBirth = DateTime.Parse("1/4/2003"),
                    Gender = "Male",
                    Address = "07619 1st Point",
                    ReceiveNewsLetters = true,
                    PersonID = Guid.Parse("bf404ca6-ca14-4f85-aaac-7e56a0c65ec1"),
                    CountryID = Guid.Parse("c8e0ef40-1a41-4810-88c7-79b274b65ecb")
                }
            };
        }
    }

    private PersonResponse ConvertPersonToPersonResponse(Person person)
    {
        PersonResponse personResponse = person.ToPersonRespone();
        personResponse.Country = _countriesService.GetCountryByCountryID(person.CountryID)?.CountryName;
        return personResponse;
    }

    public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
    {
        if (personAddRequest == null) throw new ArgumentNullException(nameof(personAddRequest));

        // if (personAddRequest.PersonName == null) throw new ArgumentException();

        ValidationHelper.ModelValidation(personAddRequest);

        Person person = personAddRequest.ToPerson();
        person.PersonID = Guid.NewGuid();

        _persons.Add(person);

        return this.ConvertPersonToPersonResponse(person);
    }

    public PersonResponse? GetPersonByPersonID(Guid? personID)
    {
        if (personID == null) return null;

        Person? person = _persons.FirstOrDefault(person => person.PersonID == personID);
        if (person == null) return null;

        return ConvertPersonToPersonResponse(person);
    }

    public List<PersonResponse> GetAllPersons()
    {
        List<PersonResponse> personResponses = _persons.Select(person => ConvertPersonToPersonResponse(person)).ToList();
        return personResponses;
    }

    public List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString)
    {
        List<PersonResponse> allPersons = GetAllPersons();
        List<PersonResponse> matchingPersons = allPersons;

        if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
            return matchingPersons;

        switch (searchBy)
        {
            case nameof(PersonResponse.PersonName):
                matchingPersons = allPersons.Where(person => (!string.IsNullOrEmpty(person.PersonName) ? person.PersonName.Contains(searchString, StringComparison.InvariantCultureIgnoreCase) : false)).ToList();
                break;
            case nameof(PersonResponse.Email):
                matchingPersons = allPersons.Where(person => (!string.IsNullOrEmpty(person.Email) ? person.Email.Contains(searchString, StringComparison.InvariantCultureIgnoreCase) : false)).ToList();
                break;
            case nameof(PersonResponse.DateOfBirth):
                matchingPersons = allPersons.Where(person => person.DateOfBirth.HasValue ? person.DateOfBirth.ToString()!.Contains(searchString, StringComparison.InvariantCultureIgnoreCase) : false).ToList();
                break;
            case nameof(PersonResponse.Gender):
                matchingPersons = allPersons.Where(person => (!string.IsNullOrEmpty(person.Gender) ? person.Gender.Contains(searchString, StringComparison.InvariantCultureIgnoreCase) : false)).ToList();
                break;
            case nameof(PersonResponse.CountryID):
            case nameof(PersonResponse.Country):
                matchingPersons = allPersons.Where(person => (!string.IsNullOrEmpty(person.Country) ? person.Country.Contains(searchString, StringComparison.InvariantCultureIgnoreCase) : false)).ToList();
                break;
            case nameof(PersonResponse.Address):
                matchingPersons = allPersons.Where(person => (!string.IsNullOrEmpty(person.Address) ? person.Address.Contains(searchString, StringComparison.InvariantCultureIgnoreCase) : false)).ToList();
                break;
            default:
                matchingPersons = allPersons;
                break;
        }

        return matchingPersons;
    }

    public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions options)
    {
        if (string.IsNullOrEmpty(sortBy))
            return allPersons;

        List<PersonResponse> sortedPersons =
        (sortBy, options) switch
        {
            (nameof(Person.PersonName), SortOrderOptions.ASCENDING) => allPersons.OrderBy(person => person.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),
            (nameof(Person.PersonName), SortOrderOptions.DESCENDING) => allPersons.OrderByDescending(person => person.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

            (nameof(Person.Email), SortOrderOptions.ASCENDING) => allPersons.OrderBy(person => person.Email, StringComparer.OrdinalIgnoreCase).ToList(),
            (nameof(Person.Email), SortOrderOptions.DESCENDING) => allPersons.OrderByDescending(person => person.Email, StringComparer.OrdinalIgnoreCase).ToList(),

            (nameof(Person.Address), SortOrderOptions.ASCENDING) => allPersons.OrderBy(person => person.Address, StringComparer.OrdinalIgnoreCase).ToList(),
            (nameof(Person.Address), SortOrderOptions.DESCENDING) => allPersons.OrderByDescending(person => person.Address, StringComparer.OrdinalIgnoreCase).ToList(),

            (nameof(Person.DateOfBirth), SortOrderOptions.ASCENDING) => allPersons.OrderBy(person => person.DateOfBirth.ToString(), StringComparer.OrdinalIgnoreCase).ToList(),
            (nameof(Person.DateOfBirth), SortOrderOptions.DESCENDING) => allPersons.OrderByDescending(person => person.DateOfBirth.ToString(), StringComparer.OrdinalIgnoreCase).ToList(),

            (nameof(PersonResponse.Age), SortOrderOptions.ASCENDING) => allPersons.OrderBy(person => person.Age).ToList(),
            (nameof(PersonResponse.Age), SortOrderOptions.DESCENDING) => allPersons.OrderByDescending(person => person.Age).ToList(),

            (nameof(PersonResponse.Gender), SortOrderOptions.ASCENDING) => allPersons.OrderBy(person => person.Gender, StringComparer.OrdinalIgnoreCase).ToList(),
            (nameof(PersonResponse.Gender), SortOrderOptions.DESCENDING) => allPersons.OrderByDescending(person => person.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

            (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.ASCENDING) => allPersons.OrderBy(person => person.ReceiveNewsLetters).ToList(),
            (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.DESCENDING) => allPersons.OrderByDescending(person => person.ReceiveNewsLetters).ToList(),

            (nameof(PersonResponse.Country), SortOrderOptions.ASCENDING) => allPersons.OrderBy(person => person.Country).ToList(),
            (nameof(PersonResponse.Country), SortOrderOptions.DESCENDING) => allPersons.OrderByDescending(person => person.Country).ToList(),

            (_, _) => allPersons
        };
        return sortedPersons;
    }

    public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
    {
        if (personUpdateRequest == null)
            throw new ArgumentNullException(nameof(Person));

        ValidationHelper.ModelValidation(personUpdateRequest);

        //get person object
        Person? matching = _persons.FirstOrDefault(person => person.PersonID == personUpdateRequest.PersonID);

        if (matching == null)
        {
            throw new ArgumentException("Given persin id does not exist");
        }

        //Update all details
        matching.PersonName = personUpdateRequest.PersonName;
        matching.Email = personUpdateRequest.Email;
        matching.DateOfBirth = personUpdateRequest.DateOfBirth;
        matching.Gender = personUpdateRequest.Gender.ToString();
        matching.CountryID = personUpdateRequest.CountryID;
        matching.Address = personUpdateRequest.Address;
        matching.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;

        return ConvertPersonToPersonResponse(matching);
    }

    public bool DeletePerson(Guid? personID)
    {
        if (!personID.HasValue)
            throw new ArgumentNullException(nameof(personID));

        Person? person = _persons.FirstOrDefault(person => person.PersonID == personID);
        if (person == null)
            return false;

        _persons.RemoveAll(person => person.PersonID == personID);
        return true;
    }
}