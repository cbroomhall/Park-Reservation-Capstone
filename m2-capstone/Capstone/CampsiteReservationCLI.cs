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
        }


        //METHODS
        public void MainMenu()
        {
            bool quit = false;
            int userNum = 0;
            while (!quit)
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("  Select a Park for Further Details");
                Console.Write(ListParks());
                Console.WriteLine("\tQ)  Quit" + "\n");
                Console.SetCursorPosition(3, parkList.Count + 4); 
                string userChoice = Console.ReadLine();
                bool validInput = Int32.TryParse(userChoice, out userNum);
                Console.WriteLine();

                if (userChoice.Equals("Q") || userChoice.Equals("q"))
                {
                    quit = true;

                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("\n \t   Thank you for using the National Parks System. Have a nice day!\n \n");
                    Console.WriteLine(@"                                  # #### ####");
                    Console.WriteLine(@"                                ### \/#|### |/####");
                    Console.WriteLine(@"                               ##\/#/ \||/##/_/##/_#");
                    Console.WriteLine(@"                             ###  \/###|/ \/ # ###");
                    Console.WriteLine(@"                           ##_\_#\_\## | #/###_/_####");
                    Console.WriteLine(@"                          ## #### # \ #| /  #### ##/##");
                    Console.WriteLine(@"                           __#_--###`  |{,###---###-~");
                    Console.WriteLine(@"                                     \ }{");
                    Console.WriteLine(@"                                      }}{");
                    Console.WriteLine(@"                                      }}{");
                    Console.WriteLine(@"                                      {{}");
                    Console.WriteLine(@"                                , -=-~{ .-^- _");
                    Console.WriteLine(@"                                      `}");
                    Console.WriteLine(@"                                       {");
                    Console.ReadLine();
                    Environment.Exit(0);

                }
                //else
                //{
                //    bool validInput = Int32.TryParse(userChoice, out userNum);

                //    if (!validInput || userNum == 0 || userNum > parkList.Count)
                //    {
                //        Console.ForegroundColor = ConsoleColor.Red;
                //        Console.WriteLine("Invalid input. Try Again.");
                //        Console.ResetColor();
                //    }
                //    else
                //    {

                //        int parkIndex = userNum - 1;
                //        int parkId = parkList[parkIndex].ParkId;
                //        ParkMenu(parkId, parkIndex);

                //    }
                //}

                else if ((!validInput || userNum == 0 || userNum > parkList.Count))
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    //Console.BackgroundColor = ConsoleColor.White;
                    Console.Write("  ***INVALID INPUT.***  "); 
                    Console.ResetColor();
                    Console.WriteLine("Press enter and try again.");
                    Console.ReadLine();
                }
                else
                {

                    int parkIndex = userNum - 1;
                    int parkId = parkList[parkIndex].ParkId;
                    Console.Clear();
                    Console.WriteLine();
                    ParkMenu(parkId, parkIndex);

                }
                
            }
        }

        private void ParkMenu(int parkId, int parkIndex)
        {
            Console.WriteLine(parkList[parkIndex].ToString());
            bool quit = false;
            while (!quit)
            {
                string message = "  Select a Command\n\t1)  View Campgrounds\n\t2)  Search for a Reservation\n\t3)  Return to Previous Screen\n\n";
                int userNum = CLIHelper.GetInt(message, 1, 3);
                //Console.SetCursorPosition(3, 13);

                switch (userNum)
                {
                    case 1:
                        Console.Clear();
                        Console.WriteLine(ListCampgrounds(parkId, parkIndex));
                        //ReservationMenu(parkId);
                        break;
                    case 2:
                        Console.Clear();
                        Console.WriteLine(ListCampgrounds(parkId, parkIndex));
                        ReservationMenu(parkId);
                        break;
                    case 3:
                        quit = true;
                        break;
                }
            }
        }

        private void ReservationMenu(int parkId)
        {
            bool quit = false;
            while (!quit)
            {
                Console.WriteLine();
                int userNum = CLIHelper.GetInt("  Select a Command\n\t1)  Search for available Reservation\n\t2)  Return to Previous Screen\n", 1, 2);
                if (userNum == 1)
                {
                    ReservationMenu2(parkId);
                }
                else
                {
                    quit = true;
                }
            }
        }

        private void ReservationMenu2(int parkId)
        {
            CampgroundSqlDAL campDalObject = new CampgroundSqlDAL(connectionString);
            List<Campground> campgroundList = campDalObject.GetCampgrounds(parkId);
            string message = "Which campground(enter 0 to cancel) ?";
            int userCampNum = CLIHelper.GetInt(message, 0, campgroundList.Count);
            int campId = campgroundList[userCampNum - 1].CampgroundID; 
            Console.WriteLine();
            List<Site> possibleSites = new List<Site>();

            ReservationSearch(parkId, campId);
        }

        private void ReservationSearch(int parkId, int campId)
        {
            bool validDates = false;
            bool quit = false;
            while (!quit)
            {

                DateTime arrival = CLIHelper.GetDateTime("What is the arrival date?  (mm/dd/yyyy)");
                Console.WriteLine();
                DateTime departure = CLIHelper.GetDateTime("What is the departure date?  (mm/dd/yyyy)");
                Console.WriteLine();
                validDates = CLIHelper.ValidDates(arrival, departure);

                if (!validDates)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write("  ***INVALID DATES***  ");
                    Console.ResetColor();
                    Console.WriteLine(" Please try again");
                    Console.WriteLine();
                }
                else
                {
                    quit = true;


                    SiteSqlDAL siteDalObject = new SiteSqlDAL(connectionString);
                    List<Site> possibleSites = siteDalObject.GetAvailableSites(campId, arrival, departure);
                    List<Site> sitesConflict = siteDalObject.GetConflictingSites(campId, arrival, departure);

                    List<Site> trueOptions = new List<Site>();
                    for (int i = 0; i < possibleSites.Count; i++)
                    {
                        bool conflict = false;
                        foreach (Site x in sitesConflict)
                        {
                            if (x.SiteId == possibleSites[i].SiteId)
                            {
                                conflict = true;
                            }
                        }
                        if (!conflict)
                        {
                            trueOptions.Add(possibleSites[i]);
                        }
                    }
                    
                    if (trueOptions.Count == 0)
                    {
                        Console.WriteLine("No sites are available for the date range you entered.");
                        bool tryNewDates = CLIHelper.yesOrNo("Enter new dates? (Y/N)");
                        if (tryNewDates)
                        {
                            quit = false;
                        }
                    }
                    else
                    {
                        quit = true;



                        int[] siteNumbers = new int[trueOptions.Count];
                        for (int i = 0; i < siteNumbers.Length; i++)
                        {
                            siteNumbers[i] = trueOptions[i].SiteNumber;
                        }
                        Console.WriteLine(ListSites(trueOptions, arrival, departure));
                        Console.WriteLine();
                        bool validSiteChoice = false;
                        int siteChoice = 0;
                        while (!validSiteChoice)
                        {
                            Console.WriteLine("Which site should be reserved (enter 0 to cancel)");
                            string input = Console.ReadLine();
                            validSiteChoice = Int32.TryParse(input, out siteChoice);
                            if (validSiteChoice && !siteNumbers.Contains(siteChoice) && siteChoice != 0)
                            {
                                validSiteChoice = false;
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.Write("***Not A Valid Site Number.*** ");
                                Console.ResetColor();
                                Console.WriteLine("Please try again");
                            }
                            else if (siteChoice != 0)
                            {
                                Console.WriteLine();
                                Console.WriteLine("What name should the reservation be made under?");
                                string reserveName = Console.ReadLine();
                                
                                ReservationSqlDAL rezDalObject = new ReservationSqlDAL(connectionString);
                                int reservationId = rezDalObject.MakeReservation(reserveName, campId, siteChoice, arrival, departure);
                                Console.WriteLine();
                                Console.WriteLine("The reservation has been made and your confirmation ID is: " + reservationId);
                                Console.WriteLine();
                            }
                        }
                    }
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

        private string ListSites(List<Site> sites, DateTime arrival, DateTime departure)
        {
            string result = "  Results Matching Your Search Criteria\n";
            result += "  Your Chosen Dates:   " + arrival.ToShortDateString() + " - " + departure.ToShortDateString() + "\n\n";
            result += "  Site No.".PadRight(12) + "Max Occup.".PadRight(12) + "Acessible?".PadRight(15) + "Max RV Length".PadRight(15) + "Utility".PadRight(12) + "Cost\n";
            for (int i = 0; i < sites.Count; i++)
            {
                result += sites[i].ToString();
            }


            return result;
        }



    }
}
