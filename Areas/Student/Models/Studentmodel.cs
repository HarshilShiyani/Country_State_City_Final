using System.ComponentModel.DataAnnotations;

namespace Country_State_City_Final.Areas.Student.Models
{
    public class Studentmodel
    {
        public int StudentId { get; set; }

        public string? StudentName { get; set; }

        public string? MobileNoStudent { get; set; }

        public string? Email { get; set; }

        public string? MobileNoFather { get; set; }

        public string? Address { get; set; }

        public DateTime BirthDate { get; set; }

        public int BranchID { get; set; }

        public int CityID { get; set; }
    }
}
