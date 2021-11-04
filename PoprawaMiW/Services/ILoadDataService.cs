using System.Collections.Generic;
using System.Threading.Tasks;

namespace PoprawaMiW.Services
{
    public interface ILoadDataService
    {
        Task<List<string>> FetchData(string fileName);
    }
}