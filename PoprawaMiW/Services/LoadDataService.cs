using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PoprawaMiW.Services
{
    public class LoadDataService : ILoadDataService
    {
        private readonly HttpClient httpClient;
        public LoadDataService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<List<string>> FetchData(string fileName)
        {
            var fetchedData = await httpClient.GetStringAsync($"dataset/{fileName}");
            var fetchedDataToList = fetchedData.Split("\n").ToList();
            var indexOfLast = fetchedDataToList.Count - 1;
            
            fetchedDataToList.RemoveAt(indexOfLast);

            return fetchedDataToList;
         }
    }
}
