using System;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts;
public interface IPersonsService
{
    /// <summary>
    /// Adds a new person into the list of people
    /// </summary>
    /// <param name="personAddRequest">Person to add</param>
    /// <returns>Returns the same person details, along with the newly generated PersonID</returns>
    PersonResponse AddPerson(PersonAddRequest? personAddRequest);

    /// <summary>
    /// return person object based off of given personId
    /// </summary>
    /// <param name="personID">Person id to search</param>
    /// <returns>Returns matching person object</returns>
    PersonResponse? GetPersonByPersonID(Guid? personID);
    /// <summary>
    /// Returns all people
    /// </summary>
    /// <returns>Returns a list of all of the people, represented as a response object</returns>
    List<PersonResponse> GetAllPersons();

    /// <summary>
    /// Returns all person objects that matches with the given search field
    /// </summary>
    /// <param name="searchBy">Search field to search</param>
    /// <param name="searchString">Search string to search</param>
    /// <returns>Returns all matching persons based on either the searchField given or searchString given</returns>
    List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString);

    /// <summary>
    /// Returns sorted list of persons
    /// </summary>
    /// <param name="allPersons">Represents list of persons to sort</param>
    /// <param name="sortBy">Name of the property from which the array will be sorted</param>
    /// <param name="options">In which order the data should be sorted</param>
    /// <returns></returns>
    List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions options);

    /// <summary>
    /// Updates the specified person details based on the given person ID
    /// </summary>
    /// <param name="personUpdateRequest">The person to update</param>
    /// <returns>person response</returns>
    PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest);

    /// <summary>
    /// Deletes person object
    /// </summary>
    /// <param name="PersonId">The person to delete</param>
    /// <returns>returns true if person was successfully deleted, false otherwise</returns>
    bool DeletePerson(Guid? personID);
}