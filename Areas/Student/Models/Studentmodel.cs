using System.ComponentModel.DataAnnotations;

namespace Country_State_City_Final.Areas.Student.Models
{
    public class Studentmodel
    {
        public int StudentId { get; set; }
        [Required(ErrorMessage ="Please select your gender")]
        public int Gender { get; set; }

        [Required]
        public string? StudentName { get; set; }
        [Required]
        [RegularExpression("^[0-9]{10}$")]
        public string? MobileNoStudent { get; set; }
        [Required]
        [RegularExpression("[a-z0-9]+@[a-z]+\\.[a-z]{2,3}", ErrorMessage = "Please enter Valid Email Address")]
        public string? Email { get; set; }
        [Required]
        [StringLength(10)]
        [RegularExpression("^[0-9]{10}$")]

        public string? MobileNoFather { get; set; }
        [Required]
        [MinLength(20)]
        public string? Address { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]

        public int BranchID { get; set; }
        [Required]

        public int CityID { get; set; }
    }
    public class CityDropDown
    {
        public int CityId { get; set; }
        public string? CityName { get; set; }
    }
    public class BranchDropDown
    {
        public int BranchId { get; set; }
        public string? BranchName { get; set; }
    }
    public class EmailModel
    {
        [Required]
        public string? To { get; set; }
        [Required]
        public string? Subject { get; set; }
        [Required]
        public string? Body { get; set; }
        [Required]
        public IFormFile? Attachment { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}