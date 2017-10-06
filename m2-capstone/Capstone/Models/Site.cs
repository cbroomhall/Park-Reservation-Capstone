using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Site
    {
        public string CampgroundName { get; set; }
        public int SiteId { get; set; }
        public int CampgroundId { get; set; }
        public int SiteNumber { get; set; }
        public int MaxOccupancy { get; set; }
        public int Accessible { get; set; }
        public int MaxRvLength { get; set; }
        public int Utilities { get; set; }



        public override string ToString()
        {
            string result = "";
            
            result += "  " + CampgroundName.PadRight(30) + SiteNumber.ToString().PadRight(5) 
                + MaxOccupancy.ToString().PadRight(5) + HandyAx(Accessible).PadRight(5) 
                + RV(MaxRvLength).PadRight(5) + Utility(Utilities);
            return result;
        }

        private string HandyAx(int x)
        {
            string result = "No";
            if (x == 1)
            {
                result = "Yes";
            }
            return result;
        }

        private string RV(int x)
        {
            string result = "N/A";
            if (x > 0)
            {
                result = x.ToString();
            }
            return result;
        }

        private string Utility(int x)
        {
            string result = "N/A";
            if (x == 1)
            {
                result = "Yes";
            }
            return result;
        }
    }
}
