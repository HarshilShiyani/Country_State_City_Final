using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Country_City_City_Final.Areas.City.Models;
using Country_State_City_Final.Areas.State.Models;
using CountryDropDownModel = Country_City_City_Final.Areas.City.Models.CountryDropDownModel;

namespace Country_State_City_Final.Areas.City.Controllers
{
    [Area("City")]
    [Route("City/[controller]/[action]")]
    public class CityController : Controller
    {
        private readonly IConfiguration _configuration;
        public CityController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public IActionResult CityList(string searchstring)
        {
            string connection = this._configuration.GetConnectionString("connectionString");
            SqlConnection sqlConnection = new SqlConnection(connection);
            sqlConnection.Open();

            SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_City_SelectAll";
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            return View("CityList", dt);
        }

        public IActionResult CityAddEdit(int? CityId)
        {
            FillCountryDDL();
            FillStateDDL();
            if (CityId != null)
            {
                string connection = this._configuration.GetConnectionString("connectionString");
                SqlConnection sqlConnection = new SqlConnection(connection);
                sqlConnection.Open();

                SqlCommand cmd = sqlConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_City_SelectByPk";
                cmd.Parameters.AddWithValue("@CityId", CityId);
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                CityModel model = new CityModel();
                foreach (DataRow dataRow in dt.Rows)
                {
                    model.CityCode = dataRow["Citycode"].ToString();
                    model.CityName = dataRow["CityName"].ToString();
                    model.StateId = Convert.ToInt32(dataRow["StateId"]);
                    model.CountryId = Convert.ToInt32(dataRow["CountryId"]);

                }
                return View("CityAddEdit", model);


            }
            return View("CityAddEdit");
        }

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

        public void FillStateDDL()
        {
            string str = this._configuration.GetConnectionString("connectionString");
            SqlConnection sqlConnection = new SqlConnection(str);
            sqlConnection.Open();

            SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_State_SelectAll";

            SqlDataReader sqlDataReader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(sqlDataReader);

            List<StateDropDownModel> Statelist = new List<StateDropDownModel>();
            foreach (DataRow dr in dt.Rows)
            {
                StateDropDownModel tempState = new StateDropDownModel();
                tempState.StateId = Convert.ToInt32(dr["StateId"]);
                tempState.StateName = dr["StateName"].ToString();
                Statelist.Add(tempState);
            }
            sqlConnection.Close();
            ViewBag.StateList = Statelist;
        }

        public IActionResult CitySave(CityModel CityModel, int CityId)
        {
            string connection = this._configuration.GetConnectionString("connectionString");
            SqlConnection sqlConnection = new SqlConnection(connection);
            sqlConnection.Open();

            SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            if (CityId == 0)
            {
                cmd.CommandText = "PR_City_Insert";
            }
            else
            {
                cmd.CommandText = "PR_City_Update";
                cmd.Parameters.AddWithValue("@CityId", CityId);
            }

            cmd.Parameters.AddWithValue("@CityName", CityModel.CityName);
            cmd.Parameters.AddWithValue("@CountryId", CityModel.CountryId);
            cmd.Parameters.AddWithValue("@StateId", CityModel.StateId);

            cmd.Parameters.AddWithValue("@Citycode", CityModel.CityCode);


            cmd.ExecuteNonQuery();
            sqlConnection.Close();

            return RedirectToAction("CityList", "City", new { area = "City" });
        }

        public IActionResult CityDelete(int CityId)
        {
            string connection = this._configuration.GetConnectionString("connectionString");
            SqlConnection sqlConnection = new SqlConnection(connection);
            sqlConnection.Open();

            SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_City_Delete";
            cmd.Parameters.AddWithValue("@CityId", CityId);
            cmd.ExecuteNonQuery();
            return RedirectToAction("CityList");
        }


    }
}
