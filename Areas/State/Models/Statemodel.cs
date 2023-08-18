using System.ComponentModel.DataAnnotations;

namespace Country_State_City_Final.Areas.State.Models
{
    public class StateModel
    {
        public int? StateId { get; set; }

        [Required(ErrorMessage = ("Please Enter Country Name"))]
        public string? StateName { get; set; }

        [Required(ErrorMessage = ("Please Select Country"))]
        public int? CountryId { get; set; }

        [Required(ErrorMessage = ("Please Enter Country Code"))]
        public string? StateCode { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        public string? searchstring { get; set; }


    }
    public class CountryDropDownModel
    {
        public int CountryId { get; set; }
        public string? CountryName { get; set; }
    }
   
}
