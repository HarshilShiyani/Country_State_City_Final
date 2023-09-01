using Country_State_City_Final.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
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
                ObjCmd.CommandText = "PR_Student_UpdateByPK";
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
            ObjCmd.ExecuteNonQuery();

            return RedirectToAction("StudentList");
        }
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

    }
}
