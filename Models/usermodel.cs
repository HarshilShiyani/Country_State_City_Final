using System.ComponentModel.DataAnnotations;

namespace Country_State_City_Final.Models
{
    public class usermodel
    {
        [Required]

        public string? username { get; set; }
        [Required]
        public string? password { get; set; }
        [Required]
        public string? email { get; set; }
        [Required]
        public string? mobileno { get; set; }
	 
    }
}
