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
        private string getReservationSql = @"select * from site join reservation on site.site_id = reservation.site_id where campground_id = @campgroundId order by site.site_number";
//        
        private string connectionString;

        // Single Parameter Constructor
        public ReservationSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Reservation> GetSites()
        {
            List<Reservation> reservationList = new List<Reservation>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(getReservationSql, conn);
                    //cmd.Parameters.AddWithValue("@campgroundId", campgroundId);
                    SqlDataReader results = cmd.ExecuteReader();

                    while (results.Read())
                    {
                        reservationList.Add(CreateReservation(results));
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return reservationList;
        }

        //HELPER METHOD
        private Reservation CreateReservation(SqlDataReader results)
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
