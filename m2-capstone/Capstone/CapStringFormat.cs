using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    public class CapStringFormat
    {
        //Method to convert integer to a string with commas inserted in the appropriate places
        public static string CommaInteger(int x)
        {
            string result = x.ToString();
            int numCommas = result.Length / 3;
            int remainder = result.Length % 3;
            int k = 0;
            for (int i = remainder; i < result.Length ; i = i + 3)
            {
                result = result.Insert(i, ",");
                k++;
                i += k;
            }
            
            return result;
        }


        //Method to insert line breaks in the long descriptions
        public static string LineBreaks(string result)
        {
            int lineCount = 0;
            //string result = x;
            int charPerLIne = 80;
            for (int i = charPerLIne; i < result.Length; i += charPerLIne)
            {
                for (int k = i; k < result.Length; k++)
                {
                    char y = result[k];
                    if (y == ' ')
                    {
                        result = result.Insert(k + 1, "\n" + "  ");
                        i = k;
                        lineCount++;
                        break;
                    }
                }
            }
            return result;
        }

        
    }
}
