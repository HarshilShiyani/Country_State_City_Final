
using System.ComponentModel.DataAnnotations;

namespace Country_City_City_Final.Areas.City.Models
{
    public class CityModel
    {
        public int? CityId { get; set; }

        [Required(ErrorMessage = ("Please Enter City Name"))]

        public string? CityName { get; set; }

        [Required(ErrorMessage = ("Please Select Country"))]
        public int? CountryId { get; set; }

        [Required(ErrorMessage = ("Please Select State"))]

        public int? StateId { get; set; }

        [Required(ErrorMessage = ("Please Enter State Code"))]

        public string? CityCode { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        public string? searchstring { get; set; }


    }
    public class CountryDropDownModel
    {
        public int CountryId { get; set; }
        public string? CountryName { get; set; }
    }
    public class StateDropDownModel
    {
        public int StateId { get; set; }
        public string? StateName { get; set; }
        public int CountryID { get; set; }

    }

}
