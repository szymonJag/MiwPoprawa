using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoprawaMiW.Models
{
    public class DecisonWithDistances
    {
        public string Decision { get; set; }
        public List<double> Distances { get; set; }

        public DecisonWithDistances(string decision, List<double>distances)
        {
            Decision = decision;
            Distances = distances;
        }
    }
}
