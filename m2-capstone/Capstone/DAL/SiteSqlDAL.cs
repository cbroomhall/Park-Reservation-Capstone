using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;
using System.Data.SqlClient;

namespace Capstone.DAL
{
    public class SiteSqlDAL
    {
        private string getSiteSql = @"select * from site where campground_id = @campgroundId order by site_number";
        private string getAvailableSitesSql = @"select * from site join reservation on site.site_id = reservation.site_id where campground_id = @campgroundId 
and @arrival > from_date and @departure > to_date order by site.site_number";
        private string connectionString;

        // Single Parameter Constructor
        public SiteSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Site> GetSites(int campgroundId)
        {
            List<Site> allSites = new List<Site>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(getSiteSql, conn);
                    cmd.Parameters.AddWithValue("@campgroundId", campgroundId);
                    SqlDataReader results = cmd.ExecuteReader();

                    while (results.Read())
                    {
                        allSites.Add(CreateSite(results));
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return allSites;
        }

        //HELPER METHOD
        private Site CreateSite(SqlDataReader results)
        {
            Site aSite = new Site();

            aSite.CampgroundName = "";
            aSite.SiteId = Convert.ToInt32(results["site_id"]);
            aSite.CampgroundId = Convert.ToInt32(results["campground_id"]);
            aSite.SiteNumber = Convert.ToInt32(results["site_number"]);
            aSite.MaxOccupancy = Convert.ToInt32(results["max_occupancy"]);
            aSite.Accessible = Convert.ToInt32(results["accessible"]);
            aSite.MaxRvLength = Convert.ToInt32(results["max_rv_length"]);
            aSite.Utilities = Convert.ToInt32(results["utilities"]);

            return aSite;
        }
    }
}
