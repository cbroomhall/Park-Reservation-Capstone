using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    class CLIHelper
    {
        public static DateTime GetDateTime(string message)
        {
            string userInput = String.Empty;
            DateTime dateValue = DateTime.MinValue;
            int numberOfAttempts = 0;

            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write("***Invalid date format.***  ");
                    Console.ResetColor();
                    Console.WriteLine(" Please try again");
                }

                Console.Write(message + " ");
                userInput = Console.ReadLine();
                numberOfAttempts++;
            }
            while (!DateTime.TryParse(userInput, out dateValue));

            return dateValue;
        }


        public static bool ValidDates(DateTime arrival, DateTime departure)
        {
            bool result = false;
            DateTime now = DateTime.Now;
            if (DateTime.Compare(arrival, now) > 0 && DateTime.Compare(departure, arrival) > 0)
            {
                result = true;
            }
            return result;
        }



        public static int GetInt(string message, int min, int max) //min and max are inclusive
        {
            string userInput = String.Empty;
            int intValue = -1;
            int numberOfAttempts = 0;
            bool validInput = false;

            while (!validInput)
            {
                if (numberOfAttempts > 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("***INVALID FORMAT.***  ");
                    Console.ResetColor();
                    Console.WriteLine(" Please try again");
                }

                Console.Write(message + " ");
                userInput = Console.ReadLine();
                if (userInput.ToLower().Equals("q"))
                {
                    return -1;
                }
                numberOfAttempts++;
                validInput = Int32.TryParse(userInput, out intValue);
                if (intValue > max || intValue < min)
                {
                    validInput = false;
                }
            }

            return intValue;
        }


        public static bool yesOrNo(string message)
        {
            bool validInput = false;
            bool result = false;
            while (!validInput)
            {
                Console.WriteLine(message);
                string input = Console.ReadLine();
                if (input.ToLower().Equals("y"))
                {
                    validInput = true;
                    result = true;
                }
                else if (input.ToLower().Equals("n"))
                {
                    validInput = true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write("INVALIDE INPUT.***  ");
                    Console.ResetColor();
                    Console.WriteLine(" Answer Y?N");
                }
            }

            return result;
        }
    }
}
