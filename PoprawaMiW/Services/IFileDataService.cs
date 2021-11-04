using PoprawaMiW.Models;
using System.Collections.Generic;

namespace PoprawaMiW.Services
{
    public interface IFileDataService
    {
        List<NumericalFileData> CreateListOfNumericalFileDatas(List<FileData> fileDatas);
    }
}