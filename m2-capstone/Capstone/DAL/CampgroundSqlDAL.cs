using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;
using System.Data.SqlClient;
using System.Configuration;

namespace Capstone.DAL
{
    public class CampgroundSqlDAL
    {
        private const string getCampgroundsSql = @"select * from campground where park_id = @parkId order by name";
        private string connectionString;

        //Single Parameter Constructor
        public CampgroundSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Campground> GetCampgrounds(int parkId)
        {
            List<Campground> campgroundList = new List<Campground>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(getCampgroundsSql, conn);
                    cmd.Parameters.AddWithValue("@parkId", parkId);
                    SqlDataReader results = cmd.ExecuteReader();

                    while (results.Read())
                    {
                        campgroundList.Add(CreateCampground(results));
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return campgroundList;
        }

        
        //HELPER METHOD
        private Campground CreateCampground(SqlDataReader results)
        {
            Campground aCampground = new Campground();

            aCampground.CampgroundID = Convert.ToInt32(results["campground_id"]);
            aCampground.ParkId = Convert.ToInt32(results["park_id"]);
            aCampground.Name = Convert.ToString(results["name"]);
            aCampground.OpenMonth = Convert.ToInt32(results["open_from_mm"]);
            aCampground.CloseMonth = Convert.ToInt32(results["open_to_mm"]);
            aCampground.DailyFee = Convert.ToDecimal(results["daily_fee"]);

            return aCampground;
        }
    }
}
