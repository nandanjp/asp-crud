using System;
using Xunit;
using System.Linq;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using Entities;
using Xunit.Abstractions;

namespace CRUDTest
{
    public class PersonsServiceTest
    {
        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _testOutputHelper;

        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            _personsService = new PersonsService(false);
            _countriesService = new CountriesService(false);
            _testOutputHelper = testOutputHelper;
        }

        #region AddPerson

        [Fact]
        public void AddPerson_NullAddThrows()
        {
            PersonAddRequest? personAddRequest = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                _personsService.AddPerson(personAddRequest);
            });
        }

        [Fact]
        public void AddPerson_PersonNameNullThrows()
        {
            PersonAddRequest? personAddRequest = new PersonAddRequest() { PersonName = null };
            Assert.Throws<ArgumentException>(() =>
            {
                _personsService.AddPerson(personAddRequest);
            });
        }

        [Fact]
        public void AddPerson_ProperPersonAdds()
        {
            PersonAddRequest? personAddRequest = new PersonAddRequest() { PersonName = "Person", Email = "example@email.com", CountryID = Guid.NewGuid(), Gender = ServiceContracts.Enums.GenderOptions.Male, DateOfBirth = DateTime.Parse("2004-01-17"), ReceiveNewsLetters = true };

            PersonResponse personResponse = _personsService.AddPerson(personAddRequest);

            List<PersonResponse> persons_list = _personsService.GetAllPersons();

            Assert.True(personResponse.PersonID != Guid.Empty);
            Assert.Contains(personResponse, persons_list);
        }

        #endregion

        #region GetAllPersonsTest

        [Fact]
        public void GetAllPersons_EmptyList()
        {
            List<PersonResponse> personResponses = _personsService.GetAllPersons();

            Assert.Empty(personResponses);
        }

        [Fact]
        public void GetAllPersons_ReturnsPeopleAdded()
        {
            CountryAddRequest country1 = new() { CountryName = "USA" };
            CountryAddRequest country2 = new() { CountryName = "Canada" };

            CountryResponse countryResponse1 = _countriesService.AddCountry(country1);
            CountryResponse countryResponse2 = _countriesService.AddCountry(country2);

            PersonAddRequest person1 = new PersonAddRequest()
            {
                PersonName = "Person 1",
                Email = "example1@email.com",
                CountryID = countryResponse1.CountryID,
                Gender = ServiceContracts.Enums.GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2004-01-17"),
                ReceiveNewsLetters = true
            };

            PersonAddRequest person2 = new PersonAddRequest()
            {
                PersonName = "Person 2",
                Email = "example2@email.com",
                CountryID = countryResponse2.CountryID,
                Gender = ServiceContracts.Enums.GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2004-01-17"),
                ReceiveNewsLetters = true
            };

            PersonAddRequest person3 = new PersonAddRequest()
            {
                PersonName = "Person 3",
                Email = "example3@email.com",
                CountryID = countryResponse2.CountryID,
                Gender = ServiceContracts.Enums.GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2004-01-29"),
                ReceiveNewsLetters = true
            };

            List<PersonResponse> personAddResponses = new() {
                _personsService.AddPerson(person1),
                _personsService.AddPerson(person2),
                _personsService.AddPerson(person3)
            };

            _testOutputHelper.WriteLine("Expected Responses:");
            foreach (PersonResponse personResponse in personAddResponses)
            {
                _testOutputHelper.WriteLine(personResponse.ToString());
            }


            List<PersonResponse> personGetAllPersons = _personsService.GetAllPersons();

            _testOutputHelper.WriteLine("Expected Responses:");
            foreach (PersonResponse personResponse in personGetAllPersons)
            {
                _testOutputHelper.WriteLine(personResponse.ToString());
            }

            foreach (PersonResponse person in personAddResponses)
            {
                Assert.Contains(person, personGetAllPersons);
            }
        }

        #endregion

        #region GetPersonByPersonId

        [Fact]
        public void GetPersonByPersonID_NullPerson()
        {
            Guid? personId = null;
            PersonResponse? personResponse =
            _personsService.GetPersonByPersonID(personId);
            Assert.Null(personResponse);
        }

        [Fact]
        public void GetPersonByPersonID_WithPersonId()
        {
            CountryAddRequest countryRequest = new CountryAddRequest() { CountryName = "Canada" };
            CountryResponse countryResponse = _countriesService.AddCountry(countryRequest);

            PersonAddRequest? personAddRequest = new PersonAddRequest() { PersonName = "Person", Email = "example@email.com", CountryID = countryResponse.CountryID, Gender = ServiceContracts.Enums.GenderOptions.Male, DateOfBirth = DateTime.Parse("2004-01-17"), ReceiveNewsLetters = true };

            PersonResponse personResponse = _personsService.AddPerson(personAddRequest);

            PersonResponse? personFromId = _personsService.GetPersonByPersonID(personResponse.PersonID);

            Assert.Equal(personResponse, personFromId);

        }

        #endregion

        #region GetFilteredPersons

        [Fact]
        public void GetFilteredPersons_EmptySearch_ReturnsAll()
        {
            List<PersonResponse> personAddResponses = AddThreePeopleToList();

            _testOutputHelper.WriteLine("Expected Responses:");
            foreach (PersonResponse personResponse in personAddResponses)
            {
                _testOutputHelper.WriteLine(personResponse.ToString());
            }

            List<PersonResponse> personGetFilteredPersons = _personsService.GetFilteredPersons(nameof(Person.PersonName), null);

            _testOutputHelper.WriteLine("Actual Responses:");
            foreach (PersonResponse personResponse in personGetFilteredPersons)
            {
                _testOutputHelper.WriteLine(personResponse.ToString());
            }

            foreach (PersonResponse person in personAddResponses)
            {
                Assert.Contains(person, personGetFilteredPersons);
            }
        }

        [Fact]
        public void GetFilteredPersons_SearchString_Works()
        {
            List<PersonResponse> personAddResponses = AddThreePeopleToList();

            _testOutputHelper.WriteLine("Expected Responses");
            foreach (PersonResponse person in personAddResponses)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }

            List<PersonResponse> personFilteredByName = _personsService.GetFilteredPersons(nameof(Person.PersonName), "ma");

            _testOutputHelper.WriteLine("Actual Responses");
            foreach (PersonResponse personFiltered in personFilteredByName)
            {
                _testOutputHelper.WriteLine(personFiltered.ToString());
            }

            foreach (PersonResponse person in personAddResponses)
            {
                if (person.PersonName != null)
                {
                    if (person.PersonName.Contains("ma", StringComparison.InvariantCultureIgnoreCase))
                    {
                        Assert.Contains(person, personFilteredByName);
                    }
                }
            }
        }

        [Fact]
        public void GetFilteredPersons_EmptyList2()
        {
            List<PersonResponse> personAddResponses = AddThreePeopleToList();

            _testOutputHelper.WriteLine("Expected Responses:");
            foreach (PersonResponse personResponse in personAddResponses)
            {
                _testOutputHelper.WriteLine(personResponse.ToString());
            }

            List<PersonResponse> personGetAllPersons = _personsService.GetAllPersons();

            _testOutputHelper.WriteLine("Expected Responses:");
            foreach (PersonResponse personResponse in personGetAllPersons)
            {
                _testOutputHelper.WriteLine(personResponse.ToString());
            }

            foreach (PersonResponse person in personAddResponses)
            {
                Assert.Contains(person, personGetAllPersons);
            }
        }

        #endregion

        #region GetSortedListofPersons
        [Fact]
        public void GetSortedPersons_DescendingWorks()
        {
            List<PersonResponse> personAddResponses = AddThreePeopleToList();
            personAddResponses = personAddResponses.OrderByDescending(person => person.PersonName).ToList();

            _testOutputHelper.WriteLine("Expected Responses:");
            foreach (PersonResponse personResponse in personAddResponses)
            {
                _testOutputHelper.WriteLine(personResponse.ToString());
            }

            List<PersonResponse> personGetSortedPersons = _personsService.GetSortedPersons(_personsService.GetAllPersons(), nameof(Person.PersonName), SortOrderOptions.DESCENDING);

            _testOutputHelper.WriteLine("Actual Responses:");
            foreach (PersonResponse personResponse in personGetSortedPersons)
            {
                _testOutputHelper.WriteLine(personResponse.ToString());
            }

            for (int i = 0; i < personAddResponses.Count; ++i)
            {
                Assert.Equal(personAddResponses.ElementAt(i), personGetSortedPersons.ElementAt(i));
            }
        }

        #endregion

        #region UpdatePerson

        [Fact]
        public void UpdatePerson_NullPerson()
        {
            PersonUpdateRequest? personUpdateRequest = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                _personsService.UpdatePerson(personUpdateRequest);
            });
        }

        [Fact]
        public void UpdatePerson_InvalidPersonID()
        {
            PersonUpdateRequest? personUpdateRequest = new() { PersonID = Guid.NewGuid() }; //not in the list of persons already

            Assert.Throws<ArgumentException>(() =>
            {
                _personsService.UpdatePerson(personUpdateRequest);
            });
        }

        [Fact]
        public void UpdatePerson_PersonNameIsNull()
        {
            CountryAddRequest countryAdd1 = new() { CountryName = "UK" };
            CountryResponse countryResponse = _countriesService.AddCountry(countryAdd1);

            PersonAddRequest personAdd = new() { PersonName = "Jotaro", CountryID = countryResponse.CountryID, Address = "Joestar house", Gender = GenderOptions.Female, ReceiveNewsLetters = true, Email = "jotaro@thejojos.com" };
            PersonResponse personResponse = _personsService.AddPerson(personAdd);

            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = null;

            Assert.Throws<ArgumentException>(() =>
            {
                _personsService.UpdatePerson(personUpdateRequest); //test that name cannot be null
            });
        }

        [Fact]
        public void UpdatePerson_PersonGetsUpdated()
        {
            CountryAddRequest countryAdd1 = new() { CountryName = "UK" };
            CountryResponse countryResponse = _countriesService.AddCountry(countryAdd1);

            PersonAddRequest personAdd = new() { PersonName = "Jotaro", CountryID = countryResponse.CountryID, Address = "Joestar house", Gender = GenderOptions.Female, ReceiveNewsLetters = true, Email = "jotaro@thejojos.com" };
            PersonResponse personResponse = _personsService.AddPerson(personAdd);

            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = "Dio";
            personUpdateRequest.Email = "muda@brando.jojo";

            PersonResponse personUpdateResponse = _personsService.UpdatePerson(personUpdateRequest);

            PersonResponse? personResponseExpected = _personsService.GetPersonByPersonID(personResponse.PersonID);

            Assert.Equal(personUpdateResponse, personResponseExpected);
        }

        #endregion

        #region DeletePerson

        [Fact]
        public void DeletePerson_ValidPersonIDDeletes()
        {
            CountryAddRequest countryAdd1 = new() { CountryName = "UK" };
            CountryResponse countryResponse = _countriesService.AddCountry(countryAdd1);

            PersonAddRequest personAdd = new() { PersonName = "Jotaro", CountryID = countryResponse.CountryID, Address = "Joestar house", Gender = GenderOptions.Female, ReceiveNewsLetters = true, Email = "jotaro@thejojos.com" };
            PersonResponse personResponse = _personsService.AddPerson(personAdd);

            bool wasDeleted = _personsService.DeletePerson(personResponse.PersonID);
            Assert.True(wasDeleted);
        }

        [Fact]
        public void DeletePerson_InvalidPersonIThrows()
        {
            CountryAddRequest countryAdd1 = new() { CountryName = "UK" };
            CountryResponse countryResponse = _countriesService.AddCountry(countryAdd1);

            PersonAddRequest personAdd = new() { PersonName = "Jotaro", CountryID = countryResponse.CountryID, Address = "Joestar house", Gender = GenderOptions.Female, ReceiveNewsLetters = true, Email = "jotaro@thejojos.com" };
            PersonResponse personResponse = _personsService.AddPerson(personAdd);

            bool wasDeleted = _personsService.DeletePerson(Guid.NewGuid());
            Assert.False(wasDeleted);
        }
        #endregion

        private List<PersonResponse> AddThreePeopleToList()
        {
            CountryAddRequest country1 = new() { CountryName = "USA" };
            CountryAddRequest country2 = new() { CountryName = "Canada" };

            CountryResponse countryResponse1 = _countriesService.AddCountry(country1);
            CountryResponse countryResponse2 = _countriesService.AddCountry(country2);

            PersonAddRequest person1 = new PersonAddRequest()
            {
                PersonName = "Person 1",
                Email = "example1@email.com",
                CountryID = countryResponse1.CountryID,
                Gender = ServiceContracts.Enums.GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2004-01-17"),
                ReceiveNewsLetters = true
            };

            PersonAddRequest person2 = new PersonAddRequest()
            {
                PersonName = "Person 2",
                Email = "example2@email.com",
                CountryID = countryResponse2.CountryID,
                Gender = ServiceContracts.Enums.GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2004-01-17"),
                ReceiveNewsLetters = true
            };

            PersonAddRequest person3 = new PersonAddRequest()
            {
                PersonName = "Person 3",
                Email = "example3@email.com",
                CountryID = countryResponse2.CountryID,
                Gender = ServiceContracts.Enums.GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2004-01-29"),
                ReceiveNewsLetters = true
            };

            List<PersonResponse> personAddResponses = new() {
                _personsService.AddPerson(person1),
                _personsService.AddPerson(person2),
                _personsService.AddPerson(person3)
            };

            return personAddResponses;
        }
    }
}