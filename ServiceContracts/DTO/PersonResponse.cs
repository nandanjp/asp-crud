using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Entities;
using ServiceContracts.Enums;

// <summary>
// DTO class used as a return type for most of CountriesService methods
// </summary>

namespace ServiceContracts.DTO
{
    /// <summary>
    /// Represents DTO class that is used as return type of most methods of Persons Service; what you want to display to the user
    /// </summary>
    public class PersonResponse
    {
        public Guid PersonID { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public Guid CountryID { get; set; } //foreign key
        public string? Country { get; set; } //foreign key
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }
        public double? Age { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(PersonResponse)) return false;

            PersonResponse other = (PersonResponse)obj;
            return (PersonID == other.PersonID && PersonName == other.PersonName && Email == other.Email && DateOfBirth == other.DateOfBirth && Gender == other.Gender && CountryID == other.CountryID && Country == other.Country && Address == other.Address && ReceiveNewsLetters == other.ReceiveNewsLetters && Age == other.Age);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"(PersonName: {PersonName}, PersonID: {PersonID}, Email: {Email}, DateOfBirth: {DateOfBirth?.ToShortDateString()}, Gender: {GenderOptions.Male.ToString()}, Country: {Country?.ToString()}, Address: {Address}, ReceiveNewsLetter: {ReceiveNewsLetters} )";
        }

        public PersonUpdateRequest ToPersonUpdateRequest()
        {
            return new PersonUpdateRequest() { PersonID = PersonID, PersonName = PersonName, Email = Email, DateOfBirth = DateOfBirth, Gender = (GenderOptions)Enum.Parse(typeof(GenderOptions), Gender!, true), Address = Address, CountryID = CountryID, ReceiveNewsLetters = ReceiveNewsLetters };
        }
    }

    public static class PersonExtensions
    {
        public static PersonResponse ToPersonRespone(this Person person)
        {
            return new PersonResponse()
            {
                PersonID = person.PersonID,
                PersonName = person.PersonName,
                Email = person.Email,
                DateOfBirth = person.DateOfBirth,
                Gender = person.Gender,
                CountryID = person.CountryID,
                Address = person.Address,
                ReceiveNewsLetters = person.ReceiveNewsLetters,
                Age = (person.DateOfBirth != null) ? Double.Floor((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25) : null,
            };
        }
    }
}