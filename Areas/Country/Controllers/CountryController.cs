using Country_State_City_Final.Areas.Country.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Country_State_City_Final.Areas.Country.Controllers
{
    [Area("Country")]
    [Route("Country/[controller]/[action]")]
    public class CountryController : Controller
    {
        private readonly IConfiguration _configuration;

        public CountryController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public IActionResult CountryList(Countrymodel countrymodel)
        {
            string connection = this._configuration.GetConnectionString("connectionString");
            SqlConnection sqlConnection = new SqlConnection(connection);

            sqlConnection.Open();

            SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[PR_Country_SelectAll]";

            DataTable dt = new DataTable();
            SqlDataReader sqlDataReader = cmd.ExecuteReader();

            dt.Load(sqlDataReader);

            List<Countrymodel> list = dt.AsEnumerable()
            .Select(row => new Countrymodel
            {
                CountryID = row.Field<int>("CountryID"),
                CountryName = row.Field<string>("CountryName"),
                CountryCode = row.Field<string>("CountryCode"),
                Created = row.Field<DateTime>("Created"),
                Modified = row.Field<DateTime>("Modified"),
            })
            .ToList();

            sqlConnection.Close();
            return View("CountryList", dt);

        }

        public IActionResult CountryAddEdit(int? CountryId)
        {
            if(CountryId != null) {
                string connection = this._configuration.GetConnectionString("connectionString");
                SqlConnection sqlConnection = new SqlConnection(connection);
                sqlConnection.Open();

                SqlCommand cmd = sqlConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_Country_SelectByPK";
                cmd.Parameters.Add("@CountryId", SqlDbType.Int).Value = CountryId;

                SqlDataReader sqlDataReader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(sqlDataReader);
                Countrymodel countrymodel = new Countrymodel();
                foreach(DataRow dataRow in dt.Rows)
                {
                    countrymodel.CountryID = Convert.ToInt32(dataRow["CountryId"]);
                    countrymodel.CountryName = dataRow["CountryName"].ToString();
                    countrymodel.CountryCode = dataRow["CountryCode"].ToString();
                }
               
                sqlConnection.Close();
                return View("CountryAddEdit", countrymodel);
            }
            return View("CountryAddEdit");
            
        }

        public IActionResult CountrySave(Countrymodel Model )
        {
            string connection = this._configuration.GetConnectionString("connectionString");
            SqlConnection sqlConnection = new SqlConnection(connection);

            sqlConnection.Open();

            SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;


            
           if (Model.CountryID == 0)
            {
                cmd.CommandText = "PR_Country_Insert";
            }
            else
            { 
                cmd.CommandText = "PR_Country_Update";
                cmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = Model.CountryID;
            }

            cmd.Parameters.Add("@CountryName", SqlDbType.VarChar).Value = Model.CountryName;
            cmd.Parameters.Add("@CountryCode", SqlDbType.VarChar).Value = Model.CountryCode;
            cmd.ExecuteNonQuery();
            return RedirectToAction("CountryList", "Country", new { area = "Country" });
        }

        public IActionResult CountryDelete(int CountryId)
        {


            string connection = this._configuration.GetConnectionString("connectionString");
            SqlConnection sqlConnection = new SqlConnection(connection);

            sqlConnection.Open();

            SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_Country_Delete";

            cmd.Parameters.AddWithValue("@CountryId", CountryId);
            cmd.ExecuteNonQuery();
            sqlConnection.Close();
            return RedirectToAction("CountryList");

        }
        public IActionResult FilterCountry()
        {
            string connection = this._configuration.GetConnectionString("connectionString");
            SqlConnection sqlConnection = new SqlConnection(connection);

            sqlConnection.Open();

            SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_LOC_Country_searchLOC_Country";
            DataTable dt = new DataTable();
            SqlDataReader sqlDataReader = cmd.ExecuteReader();

            dt.Load(sqlDataReader);

            List<Countrymodel> list = dt.AsEnumerable()
            .Select(row => new Countrymodel
            {
                CountryID = row.Field<int>("CountryID"),
                CountryName = row.Field<string>("CountryName"),
                CountryCode = row.Field<string>("CountryCode"),
                Created = row.Field<DateTime>("Created"),
                Modified = row.Field<DateTime>("Modified"),
            })
            .ToList();

            sqlConnection.Close();
            return View("CountryList", dt);

        }

    }
}
