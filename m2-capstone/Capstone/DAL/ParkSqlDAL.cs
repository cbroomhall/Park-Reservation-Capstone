using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;

namespace Capstone.DAL
{
    public class ParkSqlDAL
    {
        private const string getParkSql = "select * from park order by name";
        private string connectionString;
        //private const string getCampgroundsSql = @"select * from campground order by name";

        // Single Parameter Constructor
        public ParkSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }


        public List<Park> GetParks()
        {
            List<Park> allParks = new List<Park>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(getParkSql, conn);
                    SqlDataReader results = cmd.ExecuteReader();

                    while (results.Read())
                    {
                        allParks.Add(CreatePark(results));
                    }
                }
            }
            catch(SqlException)
            {
                throw;
            }
            return allParks;
        }
        
        



        //TO BUILD LIST OF PARK OBJECTS
        private Park CreatePark(SqlDataReader results)
        {
            Park aPark = new Park();

            aPark.ParkId = Convert.ToInt32(results["park_id"]);
            aPark.Name = Convert.ToString(results["name"]);
            aPark.Location = Convert.ToString(results["location"]);
            aPark.EstablishedDate = Convert.ToDateTime(results["establish_date"]);
            aPark.Area = Convert.ToInt32(results["area"]);
            aPark.AnnualVisitors = Convert.ToInt32(results["visitors"]);
            aPark.Description = Convert.ToString(results["description"]);

            return aPark;
        }
    }
}
