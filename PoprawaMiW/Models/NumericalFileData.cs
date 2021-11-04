using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoprawaMiW.Models
{
    public class NumericalFileData
    {
        public string Decision { get; set; }
        public Dictionary<int, double> NumericalValues { get; set; }

        public NumericalFileData(string decision, Dictionary<int, double> numericalValues)
        {
            Decision = decision;
            NumericalValues = numericalValues.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
