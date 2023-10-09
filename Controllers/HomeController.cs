using Country_State_City_Final.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Data;
using MetronicAddressBook.BAL;
using Country_State_City_Final.DAL;
using Country_State_City_Final.Areas.Student.Models;
using System.Net.Mail;
using System.Net;

namespace Country_State_City_Final.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        public HomeController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        #region Login
        public IActionResult Login(usermodel usermodel)
        {
            string connstr = this._configuration.GetConnectionString("connectionString");
            string error = null;
            if (usermodel.username == null)
            {
                error += "User Name is required";
            }
            if (usermodel.password == null)
            {
                error += "<br/>Password is required";
            }

            if (error != null)
            {
                TempData["Error"] = error;
                return RedirectToAction("Index");
            }
            else
            {
                SqlConnection sqlConnection = new SqlConnection(connstr);
                sqlConnection.Open();
                SqlCommand cmd = sqlConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_User_SelectUserByUsernamePassword";
                cmd.Parameters.AddWithValue("@password", usermodel.password);
                cmd.Parameters.AddWithValue("@email_OR_mobile_OR_username", usermodel.username);
                DataTable dt = new DataTable();
                SqlDataReader rdr = cmd.ExecuteReader();
                dt.Load(rdr);
                sqlConnection.Close();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        HttpContext.Session.SetString("username", dr["username"].ToString());
                        HttpContext.Session.SetString("email", dr["email"].ToString());
                        HttpContext.Session.SetString("password", dr["password"].ToString());
                        HttpContext.Session.SetString("mobileno", dr["mobileno"].ToString());
                        break;
                    }
                }
                else
                {
                    TempData["Error"] = "User Name or Password is invalid!";
                    return RedirectToAction("Index");
                }
                if (HttpContext.Session.GetString("username") != null && HttpContext.Session.GetString("password") != null)
                {
                    return RedirectToAction("HomePage", "Home");
                }
            }
            return RedirectToAction("Index");
        }

        #endregion

        #region Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        #endregion

        #region ForgorPassword
        public IActionResult ForgorPassword()
        {
            return View();
        }

        #endregion

        #region Index
        public IActionResult Index()
        {
            return View();
        }

        #endregion

        #region UserProfile
        public IActionResult UserProfile()
        {
            return View();
        }

        #endregion

        #region SendMail
        public void SendMail()
        {

        }

        #endregion

        #region SaveUser
        public IActionResult SaveUser(usermodel usermodel)
        {
            string conn = this._configuration.GetConnectionString("connectionString");
            SqlConnection sqlConnection = new SqlConnection(conn);
            sqlConnection.Open();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "PR_User_insert";
            sqlCommand.Parameters.AddWithValue("@username", usermodel.username);
            sqlCommand.Parameters.AddWithValue("@password", usermodel.password);
            sqlCommand.Parameters.AddWithValue("@email", usermodel.email);
            sqlCommand.Parameters.AddWithValue("@mobileno", usermodel.username);
            sqlCommand.ExecuteNonQuery();
            if (Convert.ToBoolean(sqlCommand.ExecuteNonQuery()))
            {
                TempData["IsUserAdded"] = "User Added Succesfully";
                using (MailMessage mm = new MailMessage("harshilshiyani5@gmail.com", usermodel.email.ToString()))
                {
                    mm.Subject = "Succesfully Ragisterd ";
                    mm.Body = "your username is " + usermodel.email.ToString() + " & Password is " + usermodel.password.ToString();
                    mm.IsBodyHtml = true;
                    using (SmtpClient smtp = new SmtpClient())
                    {
                        smtp.Host = "smtp.gmail.com";
                        smtp.EnableSsl = true;
                        NetworkCredential NetworkCred = new NetworkCredential("harshilshiyani5@gmail.com", "rxoqekpraeztcncr");
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = NetworkCred;
                        smtp.Port = 587;
                        smtp.Send(mm);
                    }
                }
            }

            sqlConnection.Close();

            return RedirectToAction("SignUp");
        }

        #endregion

        #region HomePage
        [CheckAccess]
        public IActionResult HomePage()
        {
            return View("HomePage");
        }
        #endregion

        #region SignUp
        public IActionResult SignUp()
        {
            return View("SignUp");
        }

        #endregion

        #region Error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion


    }
}