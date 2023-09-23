using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Net.Mail;
using System.Net;
using Country_State_City_Final.Areas.Student.Models;

namespace Country_State_City_Final.Areas.Student.Controllers
{
    [Area("Student")]
    [Route("Student/[controller]/[action]")]
    public class StudentController : Controller
    {
        public readonly IConfiguration _configuration;
        public StudentController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public IActionResult StudentList()
        {
            string connectionstr = this._configuration.GetConnectionString("connectionString");
            DataTable dt = new DataTable();
            SqlConnection sqlConnection = new SqlConnection(connectionstr);
            sqlConnection.Open();
            SqlCommand ObjCmd = sqlConnection.CreateCommand();
            ObjCmd.CommandType = CommandType.StoredProcedure;
            ObjCmd.CommandText = "PR_Student_SelectAll";
            SqlDataReader sqlDataReader = ObjCmd.ExecuteReader();
            dt.Load(sqlDataReader);
            return View(dt);
        }

        public IActionResult StudentAddEdit(int? StudentID)
        {
            FillCityDDL();
            FillBranchDDL();
            if (StudentID != null)
            {
                string connectionstr = this._configuration.GetConnectionString("connectionString");
                DataTable dt = new DataTable();
                SqlConnection sqlConnection = new SqlConnection(connectionstr);
                sqlConnection.Open();
                SqlCommand ObjCmd = sqlConnection.CreateCommand();
                ObjCmd.CommandType = CommandType.StoredProcedure;
                ObjCmd.CommandText = "PR_Student_SelectByPK";
                ObjCmd.Parameters.AddWithValue("StudentID", StudentID);
                SqlDataReader sqlDataReader = ObjCmd.ExecuteReader();
                dt.Load(sqlDataReader);
                Studentmodel model = new Studentmodel();
                foreach (DataRow dr in dt.Rows)
                {

                    model.StudentName = dr["StudentName"].ToString();
                    model.MobileNoStudent = dr["MobileNoStudent"].ToString();
                    model.Email = dr["Email"].ToString();
                    model.MobileNoFather = dr["MobileNoFather"].ToString();
                    model.Address = dr["Address"].ToString();
                    model.BirthDate = DateTime.Parse(dr["BirthDate"].ToString());
                    model.BranchID = int.Parse(dr["BranchID"].ToString());
                    model.CityID = int.Parse(dr["CityID"].ToString());

                }
                return View("StudentAddEdit", model);
            }
           
            return View("StudentAddEdit");
        }

        #region StudentSave
        public IActionResult StudentSave(Studentmodel model)
        {
            string connectionstr = this._configuration.GetConnectionString("connectionString");
            DataTable dt = new DataTable();
            SqlConnection sqlConnection = new SqlConnection(connectionstr);
            sqlConnection.Open();
            SqlCommand ObjCmd = sqlConnection.CreateCommand();
            ObjCmd.CommandType = CommandType.StoredProcedure;
            if (model.StudentId == 0)
            {
                ObjCmd.CommandText = "PR_Student_Insert";

            }
            else
            {
                ObjCmd.CommandText = "PR_Student_Update";
                ObjCmd.Parameters.AddWithValue("StudentID", model.StudentId);


            }
            ObjCmd.Parameters.AddWithValue("StudentName", model.StudentName);
            ObjCmd.Parameters.AddWithValue("MobileNoStudent", model.MobileNoStudent);
            ObjCmd.Parameters.AddWithValue("Email", model.Email);
            ObjCmd.Parameters.AddWithValue("MobileNoFather", model.MobileNoFather);
            ObjCmd.Parameters.AddWithValue("Address", model.Address);
            ObjCmd.Parameters.AddWithValue("BirthDate", model.BirthDate);
            ObjCmd.Parameters.AddWithValue("BranchID", model.BranchID);
            ObjCmd.Parameters.AddWithValue("CityID", model.CityID);
            ObjCmd.Parameters.AddWithValue("@gender", model.Gender);

            ObjCmd.ExecuteNonQuery();
            if (Convert.ToBoolean( ObjCmd.ExecuteNonQuery()) && model.StudentId == 0)
            {
                TempData["messege"] = "Succesfully inseted";
            }
            else if (Convert.ToBoolean(ObjCmd.ExecuteNonQuery()) && model.StudentId !=0)
            {
                TempData["messege"] = "Succesfully updaed";
            }
            return RedirectToAction("StudentList");
        }
        #endregion
        public IActionResult StudentDelete(int StudentId)
        {
            string connectionstr = this._configuration.GetConnectionString("connectionString");
            SqlConnection sqlConnection = new SqlConnection(connectionstr);
            sqlConnection.Open();
            SqlCommand ObjCmd = sqlConnection.CreateCommand();
            ObjCmd.CommandType = CommandType.StoredProcedure;
            ObjCmd.CommandText = "PR_Student_Delete";
            ObjCmd.Parameters.AddWithValue("StudentID", StudentId);
            ObjCmd.ExecuteNonQuery();
            return RedirectToAction("StudentList");
        }

        public void FillCityDDL()
        {

            string str = this._configuration.GetConnectionString("connectionString");
            SqlConnection sqlConnection = new SqlConnection(str);
            sqlConnection.Open();

            SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_City_CityDropdown";

            SqlDataReader sqlDataReader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(sqlDataReader);

            List<CityDropDown> citylist = new List<CityDropDown>();
            foreach (DataRow dr in dt.Rows)
            {
                CityDropDown tempcountry = new CityDropDown();
                tempcountry.CityId = Convert.ToInt32(dr["CityID"]);
                tempcountry.CityName = dr["CityName"].ToString();
                citylist.Add(tempcountry);
            }
            sqlConnection.Close();
            ViewBag.citylist = citylist;
        }
        public void FillBranchDDL()
        {

            string str = this._configuration.GetConnectionString("connectionString");
            SqlConnection sqlConnection = new SqlConnection(str);
            sqlConnection.Open();

            SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_MST_Branch_MST_BranchDropdown";

            SqlDataReader sqlDataReader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(sqlDataReader);

            List<BranchDropDown> branchlist = new List<BranchDropDown>();
            foreach (DataRow dr in dt.Rows)
            {
                BranchDropDown tempbranch = new BranchDropDown();
                tempbranch.BranchId = Convert.ToInt32(dr["BranchID"]);
                tempbranch.BranchName = dr["BranchName"].ToString();
                branchlist.Add(tempbranch);
            }
            sqlConnection.Close();
            ViewBag.branchlist = branchlist;
        }

        [HttpPost]
        public IActionResult SendMail(EmailModel emailModel)
        {
            using (MailMessage mm = new MailMessage(emailModel.Email, emailModel.To))
            {
                mm.Subject = emailModel.Subject;
                mm.Body = emailModel.Body;
                if (emailModel.Attachment.Length > -1)
                {
                    string fileName = Path.GetFileName(emailModel.Attachment.FileName);
                    mm.Attachments.Add(new Attachment(emailModel.Attachment.OpenReadStream(), fileName));
                }
                mm.IsBodyHtml = true;
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential(emailModel.Email, emailModel.Password);
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);
                    TempData["mailmessege"] = "Successfully mail sended to ";
                    @TempData["To"]=emailModel.To;  
                }
            }

            return RedirectToAction("GmailFormpage", "Student", new { area = "Student" });
        }

        public IActionResult GmailFormpage()
        { 
            return View(); 
        }


    }
}
