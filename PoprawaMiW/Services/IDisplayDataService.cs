using PoprawaMiW.Models;
using System.Collections.Generic;

namespace PoprawaMiW.Services
{
    public interface IDisplayDataService
    {
        string ShowAssignedSymbolToNumber(Dictionary<string, int> input);
        string ShowFileDataAttributes(FileData fileData);
        string ShowFrequency(Dictionary<string, int> input);
        string ShowNumercial(Dictionary<int, double> numerical);
        string ShowSymbols(Dictionary<int, string> symbols);
    }
}