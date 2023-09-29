using Microsoft.AspNetCore.Mvc;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data;
using System.Data.Common;

namespace Country_State_City_Final.DAL
{
    public class LOC_DALBase
    {
        public DataTable PR_Country_SelectAll(string conn)
        {
            try
            {
                SqlDatabase sqlDatabase = new SqlDatabase(conn);
                DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_Country_SelectAll");

                DataTable dt = new DataTable();
                using(IDataReader dataReader = sqlDatabase.ExecuteReader(dbCommand))
                {
                    dt.Load(dataReader);
                }
                return dt;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());   
                return null;
            }
        }
    }
}
