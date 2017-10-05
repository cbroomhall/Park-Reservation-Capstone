using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Campground
    {
        public int CampgroundID { get; set; }
        public int ParkId { get; set; }
        public string Name { get; set; }
        public int OpenMonth { get; set; }
        public int CloseMonth { get; set; }
        public decimal DailyFee { get; set; }

        public override string ToString()
        {
            return "\t" + Name.PadRight(40) + IntToMonth(OpenMonth).PadRight(12) + IntToMonth(CloseMonth).PadRight(12) + DailyFee.ToString("C2");
        }

        //Method to get month string from integer property
        private string IntToMonth(int month)
        {
            Dictionary<int, string> months = new Dictionary<int, string>
            {
                { 1, "January" },
                { 2, "February"},
                { 3, "March"},
                { 4, "April"},
                { 5, "May"},
                { 6, "June"},
                { 7, "July"},
                { 8, "August"},
                { 9, "September"},
                { 10, "October"},
                { 11, "November"},
                { 12, "December"}
            };
            string result = "";
            foreach (KeyValuePair<int, string> x in months)
            {
                if (month == x.Key)
                {
                    result = x.Value;
                }
            }
            return result;
        }

    }
}
