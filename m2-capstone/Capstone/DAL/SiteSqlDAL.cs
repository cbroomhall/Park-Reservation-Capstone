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
        
        
//        private string getAvailableSitesSql = @"select site.site_id, site_number, max_occupancy, accessible, max_rv_length, utilities from site 
//join reservation on site.site_id = reservation.site_id
//join campground on site.campground_id = campground.campground_id
//where site.campground_id = @campgroundId
//and ((@arrival < from_date and @departure < from_date) 
//or (@arrival > to_date and @departure > to_date) or null)
//group by site.site_id, site_number, max_occupancy, accessible, max_rv_length, utilities, daily_fee
//order by site.site_number";


        //00000000000000000000000000
        private string getSiteIdsThatConflictSql = @"select site.site_id, site_number, max_occupancy, accessible, max_rv_length, utilities from site 
join reservation on site.site_id = reservation.site_id
where site.campground_id = @campgroundId
and not((@departure <= from_date) 
or (@arrival >= to_date))
group by site.site_id, site_number, max_occupancy, accessible, max_rv_length, utilities";
        private string getSiteSql = @"select site.site_id, site_number, max_occupancy, accessible, max_rv_length, utilities from site 
where site.campground_id = @campgroundId
group by site.site_id, site_number, max_occupancy, accessible, max_rv_length, utilities";
        //00000000000000000000000000000



        private string connectionString;

        // Single Parameter Constructor
        public SiteSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        //public List<Site> GetAvailableSites(int campgroundId, DateTime arrival, DateTime departure)
        //{
        //    List<Site> allSites = new List<Site>();

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(connectionString))
        //        {
        //            conn.Open();

        //            SqlCommand cmd = new SqlCommand(getAvailableSitesSql, conn);
        //            cmd.Parameters.AddWithValue("@campgroundId", campgroundId);
        //            cmd.Parameters.AddWithValue("@arrival", arrival);
        //            cmd.Parameters.AddWithValue("@departure", departure);
        //            SqlDataReader results = cmd.ExecuteReader();

        //            while (results.Read())
        //            {
        //                allSites.Add(CreateSite(results));
        //            }
        //        }
        //    }
        //    catch (SqlException)
        //    {
        //        throw;
        //    }
        //    return allSites;
        //}

        //GET ALL SITES!!!!
        public List<Site> GetAvailableSites(int campgroundId, DateTime arrival, DateTime departure)
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



        //GET CONFLICTING SITES
        public List<Site> GetConflictingSites(int campgroundId, DateTime arrival, DateTime departure)
        {
            List<Site> listOfSitesReserved = new List<Site>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(getSiteIdsThatConflictSql, conn);
                    cmd.Parameters.AddWithValue("@campgroundId", campgroundId);
                    cmd.Parameters.AddWithValue("@arrival", arrival);
                    cmd.Parameters.AddWithValue("@departure", departure);
                    SqlDataReader results = cmd.ExecuteReader();

                    while (results.Read())
                    {
                        listOfSitesReserved.Add(CreateSite(results));
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return listOfSitesReserved;
        }









        

        //HELPER METHOD
        private Site CreateSite(SqlDataReader results)
        {
            Site aSite = new Site();

            aSite.CampgroundName = "";
            aSite.SiteId = Convert.ToInt32(results["site_id"]);
            //aSite.CampgroundId = Convert.ToInt32(results["campground_id"]);
            aSite.SiteNumber = Convert.ToInt32(results["site_number"]);
            aSite.MaxOccupancy = Convert.ToInt32(results["max_occupancy"]);
            aSite.Accessible = Convert.ToInt32(results["accessible"]);
            aSite.MaxRvLength = Convert.ToInt32(results["max_rv_length"]);
            aSite.Utilities = Convert.ToInt32(results["utilities"]);
            //aSite.DailyFee = Convert.ToDecimal(results["daily_fee"]);

            return aSite;
        }
    }
}
