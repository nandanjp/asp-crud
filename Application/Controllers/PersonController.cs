using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

[Controller]
[Route("persons")]
public class PersonController : Controller
{
    private readonly IPersonsService _personsService;
    private readonly ICountriesService _countriesService;

    public PersonController(IPersonsService personsService, ICountriesService countriesService)
    {
        _personsService = personsService;
        _countriesService = countriesService;
    }


    [Route("/")]
    [Route("[action]")]
    public IActionResult Index(string searchBy, string? searchString, string sortBy = nameof(PersonResponse.PersonName), SortOrderOptions sortOrder = SortOrderOptions.ASCENDING)
    {
        ViewBag.Title = "Persons";

        ViewBag.SearchFields = new Dictionary<string, string>()
        {
            {nameof(PersonResponse.PersonName), "Person Name"},
            {nameof(PersonResponse.Email), "Email"},
            {nameof(PersonResponse.DateOfBirth), "DateOfBirth"},
            {nameof(PersonResponse.Gender), "Gender"},
            {nameof(PersonResponse.CountryID), "Country"},
            {nameof(PersonResponse.Address), "Address"},
        };

        //Filter
        List<PersonResponse> persons = _personsService.GetFilteredPersons(searchBy, searchString);

        //Sort
        List<PersonResponse> sortedPersons = _personsService.GetSortedPersons(persons, sortBy, sortOrder);

        ViewBag.CurrentSearchBy = searchBy;
        ViewBag.CurrentSearchString = searchString?.Substring(0, searchString.Length);
        ViewBag.CurrentSortBy = sortBy;
        ViewBag.CurrentSortOrder = sortOrder;

        return View(sortedPersons);
    }

    [Route("[action]")]
    [HttpGet]
    public IActionResult Create()
    {
        List<CountryResponse> countryResponses = _countriesService.GetAllCountries();
        ViewBag.Countries = countryResponses.Select(country => new SelectListItem()
        {
            Text = country.CountryName,
            Value = country.CountryID.ToString()
        }).ToList(); //generates options tag

        return View();
    }

    [Route("[action]")]
    [HttpPost]
    public IActionResult Create(PersonAddRequest person)
    {
        if (!ModelState.IsValid)
        {
            List<CountryResponse> countryResponses = _countriesService.GetAllCountries();
            ViewBag.Countries = countryResponses;
            ViewBag.Errors = ModelState.Values.SelectMany(val => val.Errors).Select(e => e.ErrorMessage).ToList();
            return View();
        }

        PersonResponse personResponse = _personsService.AddPerson(person);

        return RedirectToAction("Index", "Person");
    }

    [Route("[action]/{personID}")]
    [HttpGet]
    public IActionResult Edit([FromRoute] Guid personID)
    {
        PersonResponse? personResponse = _personsService.GetPersonByPersonID(personID);

        if (personResponse == null)
        {
            return RedirectToAction("Index", "Person");
        }

        PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();
        List<CountryResponse> countryResponses = _countriesService.GetAllCountries();
        ViewBag.Countries = countryResponses.Select(country => new SelectListItem()
        {
            Text = country.CountryName,
            Value = country.CountryID.ToString()
        }).ToList(); //generates options tag

        return View(personUpdateRequest);
    }

    [HttpPost]
    [Route("[action]/{personID}")]
    public IActionResult Edit(PersonUpdateRequest personUpdate)
    {
        PersonResponse? person = _personsService.GetPersonByPersonID(personUpdate.PersonID);
        if (person == null)
        {
            return RedirectToAction("Index", "Person");
        }

        if (ModelState.IsValid)
        {
            PersonResponse updated = _personsService.UpdatePerson(personUpdate);
            return RedirectToAction("Index", "Person");
        }

        List<CountryResponse> countryResponses = _countriesService.GetAllCountries();
        ViewBag.Countries = countryResponses.Select(country => new SelectListItem()
        {
            Text = country.CountryName,
            Value = country.CountryID.ToString()
        }).ToList(); //generates options tag

        ViewBag.Errors = ModelState.Values.SelectMany(val => val.Errors).Select(e => e.ErrorMessage).ToList();
        return View(person.ToPersonUpdateRequest());
    }

    [Route("[action]/{personID}")]
    [HttpGet]
    public IActionResult Delete([FromRoute] Guid? personID)
    {
        PersonResponse? personResponse = _personsService.GetPersonByPersonID(personID);

        if (personResponse == null)
        {
            return RedirectToAction("Index", "Person");
        }

        return View(personResponse);
    }

    [HttpPost]
    [Route("[action]/{personID}")]
    public IActionResult Delete(PersonUpdateRequest person)
    {
        PersonResponse? personToDelete = _personsService.GetPersonByPersonID(person.PersonID);
        if (personToDelete == null)
        {
            return RedirectToAction("Index", "Person");
        }

        bool deleted = _personsService.DeletePerson(person.PersonID);
        return RedirectToAction("Index", "Person");
    }
}