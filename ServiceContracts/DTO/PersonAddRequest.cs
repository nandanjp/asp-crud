using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

using Entities;
using ServiceContracts.Enums;

//DTO is the data that should be entered by the user
//Entity is the data stored in the actual database

// <summary>
// DTO class used as a return type for most of PeopleService methods
// </summary>

namespace ServiceContracts.DTO
{
    public class PersonAddRequest
    {
        [Required(ErrorMessage = "Person name cannot be blank")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "Email cannot be blank")]
        [EmailAddress(ErrorMessage = "Email value should be a valid email")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
        [Required(ErrorMessage = "Please Select a Gender")]
        public GenderOptions? Gender { get; set; }
        public Guid CountryID { get; set; } //foreign key
        [Required(ErrorMessage = "Please Provide an Address")]
        public string? Address { get; set; }
        [Required(ErrorMessage = "Please Specify if you want to receive news letters")]
        public bool ReceiveNewsLetters { get; set; }

        public Person ToPerson()
        {
            return new Person()
            {
                PersonName = this.PersonName,
                Email = this.Email,
                DateOfBirth = this.DateOfBirth,
                Gender = this.Gender.ToString(),
                Address = this.Address,
                CountryID = this.CountryID,
                ReceiveNewsLetters = this.ReceiveNewsLetters
            };
        }
    }
}