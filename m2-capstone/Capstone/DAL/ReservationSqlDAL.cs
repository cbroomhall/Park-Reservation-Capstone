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
    public class ReservationSqlDAL
    {
        //private string getReservationSql = @"select * from site join reservation on site.site_id = reservation.site_id where campground_id = @campgroundId order by site.site_number";
        private string makeReservationSql = @"insert into reservation values 
((select site_id from site where site_number = @siteNum and campground_id = @campgroundId), @reserveName, @arrival, @departure, default)";
        private string getRezIdSql = @"select reservation_id from reservation where site_id = 
(select site_id from site where site_number = @siteNum and campground_id = @campgroundId) and name = @reserveName and from_date = @arrival and to_date = @departure";
        private string connectionString;

        // Single Parameter Constructor
        public ReservationSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public int MakeReservation(string reserveName, int campgroundId, int siteNum, DateTime arrival, DateTime departure)
        {
            int result = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(makeReservationSql, conn);
                    cmd.Parameters.AddWithValue("@siteNum", siteNum);
                    cmd.Parameters.AddWithValue("@campgroundId", campgroundId);
                    cmd.Parameters.AddWithValue("@reserveName", reserveName);
                    cmd.Parameters.AddWithValue("@arrival", arrival);
                    cmd.Parameters.AddWithValue("@departure", departure);
                    SqlDataReader results = cmd.ExecuteReader();
                    

                }
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(getRezIdSql, conn);
                    cmd.Parameters.AddWithValue("@siteNum", siteNum);
                    cmd.Parameters.AddWithValue("@campgroundId", campgroundId);
                    cmd.Parameters.AddWithValue("@reserveName", reserveName);
                    cmd.Parameters.AddWithValue("@arrival", arrival);
                    cmd.Parameters.AddWithValue("@departure", departure);
                    result = (int)cmd.ExecuteScalar();
                }
            }
            catch (SqlException)
            {
                throw;
            }



            return result;
        }


        //public string GetReservation(int reservationId)
        //{

        //}
        //public List<Reservation> GetSites()
        //{
        //    List<Reservation> reservationList = new List<Reservation>();

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(connectionString))
        //        {
        //            conn.Open();

        //            SqlCommand cmd = new SqlCommand(getReservationSql, conn);
        //            //cmd.Parameters.AddWithValue("@campgroundId", campgroundId);
        //            SqlDataReader results = cmd.ExecuteReader();

        //            while (results.Read())
        //            {
        //                reservationList.Add(CreateReservation(results));
        //            }
        //        }
        //    }
        //    catch (SqlException)
        //    {
        //        throw;
        //    }
        //    return reservationList;
        //}

        //HELPER METHOD
        public Reservation CreateReservation(SqlDataReader results)
        {
            Reservation reservy = new Reservation();

            reservy.ReservationId = Convert.ToInt32(results["reservation_id"]);
            reservy.SiteId = Convert.ToInt32(results["site_id"]);
            reservy.Name = Convert.ToString(results["name"]);
            reservy.FromDate = Convert.ToDateTime(results["from_date"]);
            reservy.ToDate = Convert.ToDateTime(results["to_date"]);
            reservy.CreateDate = Convert.ToDateTime(results["create_date"]);

            return reservy;
        }

    }
}
