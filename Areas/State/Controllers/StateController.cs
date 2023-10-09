using Country_State_City_Final.Areas.Country.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Country_State_City_Final.Areas.State.Models;
using System.Reflection;

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

        #region StateAddEdit
        public IActionResult StateAddEdit(int? StateId)
        {

            FillCountryDDL();
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

        #endregion

        #region StateDelete
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

        #endregion

        #region FillCountryDDL
        public void FillCountryDDL()
        {

            string str = this._configuration.GetConnectionString("connectionString");
            SqlConnection sqlConnection = new SqlConnection(str);
            sqlConnection.Open();

            SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_Country_SelectAll";

            SqlDataReader sqlDataReader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(sqlDataReader);

            List<CountryDropDownModel> countrylist = new List<CountryDropDownModel>();
            foreach (DataRow dr in dt.Rows)
            {
                CountryDropDownModel tempcountry = new CountryDropDownModel();
                tempcountry.CountryId = Convert.ToInt32(dr["CountryId"]);
                tempcountry.CountryName = dr["CountryName"].ToString();
                countrylist.Add(tempcountry);
            }
            sqlConnection.Close();
            ViewBag.CountryList = countrylist;
        }

        #endregion

        #region StateList
        public IActionResult StateList(string serchstring)
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

        #endregion

        #region StateSave
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


            if (Convert.ToBoolean(cmd.ExecuteNonQuery()) && StateId != 0)
            {
                TempData["stateaddeditmessage"] = "State edited succesfullly";
            }
            else
            {
                TempData["stateaddeditmessage"] = "New State Added succesfullly";
            }
            sqlConnection.Close();

            return RedirectToAction("StateList", "State", new { area = "State" });
        }

        #endregion
    }
}
