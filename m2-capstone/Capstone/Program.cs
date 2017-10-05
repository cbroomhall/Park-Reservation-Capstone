using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;
using Capstone.DAL;

namespace Capstone
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Sample Code to get a connection string from the
            // App.Config file
            // Use this so that you don't need to copy your connection string all over your code!
            string connectionString = ConfigurationManager.ConnectionStrings["CampgroundConnection"].ConnectionString;
            ParkSqlDAL myObject = new ParkSqlDAL(connectionString);
            List<Park> myParks = myObject.GetParks();
            CampsiteReservationCLI cliObject = new CampsiteReservationCLI(myParks);
            cliObject.MainMenu();
            
        }
    }
}
