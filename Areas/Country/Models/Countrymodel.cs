using System.ComponentModel.DataAnnotations;

namespace Country_State_City_Final.Areas.Country.Models
{
    public class Countrymodel
    {
        public int CountryID { get; set; }

        [Required(ErrorMessage =("Please Enter Country Name"))]
        public string? CountryName { get; set; }

        [Required(ErrorMessage = ("Please Enter Country Code"))]
        public string? CountryCode { get; set;}
        public DateTime Created { get; set;}  
        public DateTime Modified { get; set;}

      

    }
}
