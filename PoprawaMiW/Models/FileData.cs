using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PoprawaMiW.Helpers;

namespace PoprawaMiW.Models
{
    public class FileData
    {
        public string Decision { get; set; }
        public List<string> Attributes { get; set; }
        public Dictionary<int, double> NumericalValues { get; set; } = new Dictionary<int, double>();
        public Dictionary<int, string> SymbolicValues { get; set; } = new Dictionary<int, string>();

        public FileData(string line, int indexOfDecision)
        {
            var values = line.Split(' ');
            Attributes = values.ToList();
            this.Decision = Attributes[indexOfDecision];
            Attributes.Remove(Attributes[indexOfDecision]);

            for(int i = 0; i < Attributes.Count; i++)
            {
                try
                {
                    var result = StringHelpers.Parse(Attributes.ElementAt(i));
                    NumericalValues.Add(i, result);
                }
                catch(Exception e) 
                {
                    SymbolicValues.Add(i, Attributes[i]);
                }
            }
        }
    }
}
