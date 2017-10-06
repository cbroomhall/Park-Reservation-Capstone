using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Park
    {
        public int ParkId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime EstablishedDate { get; set; }
        public int Area { get; set; }
        public int AnnualVisitors { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return "  " + Name.ToString() + "\n  " + "Location:".PadRight(20) + Location + "\n  " + "Established:".PadRight(20) 
                + EstablishedDate.ToShortDateString() + "\n  " + "Area:".PadRight(20) + CapStringFormat.CommaInteger(Area) + " sq km\n  " + "Annual Visitors:".PadRight(20) 
                + CapStringFormat.CommaInteger(AnnualVisitors) + "\n\n" + CapStringFormat.LineBreaks(Description) + "\n\n";
        }

    }
}
