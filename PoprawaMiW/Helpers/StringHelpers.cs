using PoprawaMiW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoprawaMiW.Helpers
{
    public static class StringHelpers
    {
        public static double Parse(string value)
        {
            double result = 0;

            if(value!=" ")
            {
                if (!double.TryParse(value.Replace('.', ','), out result))
                {
                    if (!double.TryParse(value.Replace(',', '.'), out result))
                    {
                        throw new System.Exception($"Wartosc {result} nie jest liczba");
                    }
                }
            }
            return result;
        }
    }
}
