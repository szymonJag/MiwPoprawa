using PoprawaMiW.Models;
using System.Collections.Generic;

namespace PoprawaMiW.Services
{
    public interface IKnnService
    {
        int FindSmallestKForFirstWay(List<NumericalFileData> fileDatas);
        int FindSmallestKForSecondWay(List<NumericalFileData> fileDatas);
    }
}