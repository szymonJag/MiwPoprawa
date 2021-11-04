using PoprawaMiW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoprawaMiW.Services
{
    public class DisplayDataService : IDisplayDataService
    {
        public string ShowFileDataAttributes(FileData fileData)
        {
            var outputString = fileData.Decision;
            var attributes = fileData.Attributes;
            var lastAttribute = attributes[attributes.Count - 1];

            foreach (var attribute in attributes)
            {
                if (attribute != lastAttribute)
                {
                    outputString += $"{attribute} ";
                }
                else
                {
                    outputString += $"{attribute}";
                }
            }

            return outputString;
        }
        public string ShowFrequency(Dictionary<string, int> input)
        {
            var output = "";

            foreach(var x in input)
            {
                output += $"{x.Key} wystapiło {x.Value}\n";
            }

            return output;
        }
        public string ShowAssignedSymbolToNumber(Dictionary<string, int> input)
        {
            var output = "";
            foreach(var x in input)
            {
                output += $"{x.Key} przypisano {x.Value} | ";
            }

            return output;
        }
        public string ShowNumercial(Dictionary<int, double> numerical)
        {
            var output = "";

            foreach(var x in numerical)
            {
                output += $"id: {x.Key} value: {x.Value} | ";
            }

            return output;
        }
        public string ShowSymbols(Dictionary<int, string> symbols)
        {
            var output = "";

            foreach (var x in symbols)
            {
                output += $"id: {x.Key} value: {x.Value} | ";
            }

            return output;
        }
    }
}
