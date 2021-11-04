using PoprawaMiW.Models;
using System.Collections.Generic;

namespace PoprawaMiW.Services
{
    public interface INormalizeService
    {
        List<NumericalFileData> NormalizeData(List<NumericalFileData> fileDatas, double maxRange, double minRange);
    }
}