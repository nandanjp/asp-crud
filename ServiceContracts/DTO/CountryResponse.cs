using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Entities;

// <summary>
// DTO class used as a return type for most of CountriesService methods
// </summary>

namespace ServiceContracts.DTO
{

    public class CountryResponse
    {
        public Guid CountryID { get; set; }
        public string? CountryName { get; set; }
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;

            if (obj.GetType() != typeof(CountryResponse)) return false;

            CountryResponse other = (CountryResponse)obj;

            return this.CountryName!.Equals(other.CountryName, StringComparison.InvariantCulture) && this.CountryID == other.CountryID;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
    public static class CountryExtensions
    {
        //Syntax: the  'this' or the caller object type is Country
        public static CountryResponse ToCountryResponse(this Country country)
        {
            return new CountryResponse()
            {
                CountryID = country.CountryID,
                CountryName = country.CountryName
            };
        }
    }
}