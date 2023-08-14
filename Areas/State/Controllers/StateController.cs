using Country_State_City_Final.Areas.Country.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Country_State_City_Final.Areas.State.Models;

namespace Country_State_City_Final.Areas.State.Controllers
{
    [Area("State")]
    [Route("State/[controller]/[action]")]
    public class StateController : Controller
    {

        private readonly IConfiguration _configuration;
        public StateController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public IActionResult StateAddEdit(int? StateId)
        {
            if (StateId != null)
            {
                string connection = this._configuration.GetConnectionString("connectionString");
                SqlConnection sqlConnection = new SqlConnection(connection);
                sqlConnection.Open();

                SqlCommand cmd = sqlConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_STATE_SelectByPk";
                cmd.Parameters.AddWithValue("@StateId", StateId);
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                StateModel model = new StateModel();
                foreach (DataRow dataRow in dt.Rows)
                {
                    model.StateCode = dataRow["Statecode"].ToString();
                    model.StateName = dataRow["StateName"].ToString();
                    model.CountryId = Convert.ToInt32(dataRow["CountryId"]);
                }
                return View("StateAddEdit", model);


            }
            return View("StateAddEdit");
        }

        public IActionResult StateDelete(int StateId)
        {
            string connection = this._configuration.GetConnectionString("connectionString");
            SqlConnection sqlConnection = new SqlConnection(connection);
            sqlConnection.Open();

            SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_State_Delete";
            cmd.Parameters.AddWithValue("@StateId", StateId);
            cmd.ExecuteNonQuery();
            return RedirectToAction("StateList");
        }


        public void FillCountryDDL()
        {

            string str =this._configuration.GetConnectionString("connectionString");

            List<LOC_CountryDropDownModel> countrylistfordropdown = new List<LOC_CountryDropDownModel>();

            SqlConnection sqlConnection = new SqlConnection(str);
            sqlConnection.Open();

            SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_Country_SelectAll";

            SqlDataReader objSDR = cmd.ExecuteReader();

            if (objSDR.HasRows)
            {
                while (objSDR.Read())
                {
                    LOC_CountryDropDownModel country = new
                        LOC_CountryDropDownModel()
                    {
                        CountryId = Convert.ToInt32(objSDR["CountryId"]),
                        CountryName = objSDR["CountryName"].ToString()
                    };
                    countrylistfordropdown.Add(country);
                }
                objSDR.Close();
            }
            sqlConnection.Close();
            ViewBag.CountryList = countrylistfordropdown;

        }


        public IActionResult StateList()
        {
            string connection = this._configuration.GetConnectionString("connectionString");
            SqlConnection sqlConnection = new SqlConnection(connection);
            sqlConnection.Open();

            SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_State_SelectAll";
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            return View("StateList", dt);
        }

        public IActionResult StateSave(StateModel stateModel, int StateId)
        {
            string connection = this._configuration.GetConnectionString("connectionString");
            SqlConnection sqlConnection = new SqlConnection(connection);
            sqlConnection.Open();

            SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            if (StateId == 0)
            {
                cmd.CommandText = "PR_State_Insert";
            }
            else
            {
                cmd.CommandText = "PR_State_Update";
                cmd.Parameters.AddWithValue("@stateId", StateId);
            }

            cmd.Parameters.AddWithValue("@StateName", stateModel.StateName);
            cmd.Parameters.AddWithValue("@CountryId", stateModel.CountryId);
            cmd.Parameters.AddWithValue("@Statecode", stateModel.StateCode);


            cmd.ExecuteNonQuery();
            sqlConnection.Close();

            return RedirectToAction("StateList", "State", new { area = "State" });
        }
    }
}
