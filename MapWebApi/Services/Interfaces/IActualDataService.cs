using System.Collections.Generic;
using System.Threading.Tasks;
using MapWebApi.Models;

namespace MapWebApi.Services.Interfaces
{
    public interface IActualDataService
    {
        public void SeedData();
        public Task<List<ActualData>> GetActualData();
        public Task<ActualData> GetActualDataById(int id);
        public Task<bool> DeleteAsync(int id);
    }
}