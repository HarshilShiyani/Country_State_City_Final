using Humanizer;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Country_State_City_Final.Models
{
    public class usermodel
    {
        public usermodel()
        {
            password = GenerateRandomPassword();
        }

        [Required(ErrorMessage = "Please Enter Username/Email")]
        
        public string? username { get; set; }
        [Required]
        public string? password { get; set; }
        [Required]
        public string? email { get; set; }
        [Required]
        public string? mobileno { get; set; }

        private string GenerateRandomPassword()
        {
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder passwordBuilder = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < 10; i++) // You can adjust the password length as needed
            {
                int index = random.Next(validChars.Length);
                passwordBuilder.Append(validChars[index]);
            }

            return passwordBuilder.ToString();
        }
    }
}
    




