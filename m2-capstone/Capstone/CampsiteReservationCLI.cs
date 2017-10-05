using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;
using Capstone.DAL;
using System.Configuration;

namespace Capstone
{
    public class CampsiteReservationCLI
    {
        private List<Park> parkList;
        private string connectionString = ConfigurationManager.ConnectionStrings["CampgroundConnection"].ConnectionString;

        //CONSRUCTOR
        public CampsiteReservationCLI(List<Park> parks)
        {
            parkList = parks;
            //connectionString = 
        }


        //METHODS
        public void MainMenu()
        {
            bool quit = false;
            int userNum = 0;
            while (!quit)
            {
                Console.Clear();
                Console.WriteLine("Select a Park for Further Details");
                Console.Write(ListParks());
                Console.WriteLine("\tQ)  Quit" + "\n");
                string userChoice = Console.ReadLine();
                Console.WriteLine();

                if (userChoice.Equals("Q") || userChoice.Equals("q"))
                {
                    quit = true;
                }
                else
                {
                    bool validInput = Int32.TryParse(userChoice, out userNum);

                    if (!validInput || userNum == 0 || userNum > parkList.Count)
                    {
                        Console.WriteLine("Invalid input. Try Again.");
                    }
                    else
                    {
                        int parkIndex = userNum - 1;
                        int parkId = parkList[parkIndex].ParkId;
                        ParkMenu(parkId, parkIndex);
                    }
                }

            }
        }

        private void ParkMenu(int parkId, int parkIndex)
        {
            Console.WriteLine(parkList[parkIndex].ToString());
            bool quit = false;
            while (!quit)
            {
                Console.WriteLine("Select a Command");
                Console.WriteLine("\t1)  View Campgrounds");
                Console.WriteLine("\t2)  Search for a Reservation");
                Console.WriteLine("\t3)  Return to Previous Screen");
                Console.WriteLine();
                int userNum = 0;
                string userChoice = Console.ReadLine();
                Console.WriteLine();
                bool validInput = Int32.TryParse(userChoice, out userNum);
                if (validInput && userNum > 0 && userNum < 4)
                {
                    switch (userNum)
                    {
                        case 1:
                            Console.WriteLine(ListCampgrounds(parkId, parkIndex));
                            break;
                        case 2:
                            Console.WriteLine(ListCampgrounds(parkId, parkIndex));
                            ReservationMenu(parkId);
                            break;
                        case 3:
                            quit = true;
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Try Again.");
                }
            }
        }

        private void ReservationMenu(int parkId)
        {
            bool quit = false;
            while (!quit)
            {
                Console.WriteLine("Select a Command");
                Console.WriteLine("\t1)  Search for available Reservation");
                Console.WriteLine("\t2)  Return to Previous Screen");
                Console.WriteLine();
                int userNum = 0;
                string userChoice = Console.ReadLine();
                Console.WriteLine();
                bool validInput = Int32.TryParse(userChoice, out userNum);
                if (validInput && userNum > 0 && userNum < 3)
                {
                    switch(userNum)
                    {
                        case 1:
                            CampgroundSqlDAL dalObject = new CampgroundSqlDAL(connectionString);
                            List<Campground> campgroundList = dalObject.GetCampgrounds(parkId);
                            int userCampNum = -1;
                            DateTime arrival;
                            DateTime departure;
                            Console.WriteLine();
                            while (userCampNum == -1)
                            {
                                Console.WriteLine("Which campground (enter 0 to cancel)?");
                                string userCampground = Console.ReadLine();
                                userCampNum = CheckIntInput(0, campgroundList.Count, userCampground);
                            }
                            if (userCampNum == 0)
                            {
                                quit = true;
                            }
                            else
                            {
                                Console.WriteLine("What is the arrival date?  (mm/dd/yyyy)");
                                string userArrival = Console.ReadLine();
                                bool arrivalParse = DateTime.TryParse(userArrival, out arrival);
                                Console.WriteLine("What is the departure date?  (mm/dd/yyyy)");
                                string userDeparture = Console.ReadLine();
                                bool departureParse = DateTime.TryParse(userDeparture, out departure);
                                //DOES NOT EXIST YET
                                //List<Site> = MakeReservation(userCampNum, arrival, departure);
                            }
                            break;
                        case 2:
                            quit = true;
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Try Again.");
                }
            }
        }

        private string ListParks()
        {

            string result = "";

            int i = 1;
            foreach (Park x in parkList)
            {
                result += "\t" + i + ")  " + x.Name + "\n";
                i++;
            }

            return result;
        }

        private string ListCampgrounds(int parkId, int parkIndex)
        {
            CampgroundSqlDAL dalObject = new CampgroundSqlDAL(connectionString);
            List<Campground> campgroundList = dalObject.GetCampgrounds(parkId);

            string result = "";
            result += parkList[parkIndex].Name + " National Park Campgrounds\n\n";
            result += "\tName".PadRight(41) + "Open".PadRight(12) + "Close".PadRight(12) + "Daily Fee\n";
            for (int i = 0; i < campgroundList.Count; i++)
            {
                result += "  #" + (i + 1) + " " + campgroundList[i].ToString();
                result += "\n";
            }

            return result;
        }


        //Helper method
        private int CheckIntInput(int min, int max, string checking)
        {
            int result = -1;
            bool canParse = Int32.TryParse(checking, out result);
            
            if (!canParse || result < min || result > max)
            {
                result = -1;
                Console.WriteLine("Invalid input. Try Again.");
            }
            return result;
        }
    }
}
