using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Entities;

// <summary>
// DTO for Country
// </summary>

namespace ServiceContracts.DTO;
public class CountryAddRequest
{
    public string? CountryName { get; set; } //countryid not apart of the dto/request
    public Country ToCountry()
    {
        return new Country()
        {
            CountryName = CountryName
        };
    }
}