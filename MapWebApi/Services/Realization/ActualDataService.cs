using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapWebApi.Models;
using MapWebApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MapWebApi.Services.Realization
{
    public class ActualDataService:IActualDataService
    {
        private SubdivisionsContext _context;
        public ActualDataService(SubdivisionsContext context)
        {
            _context = context;
        }
        public void SeedData()
        {
            if (_context.ActualData.Count()<=0)
            {
                ActualData firstActualData = new ActualData()
                {
                    Date = new DateTime(1941, 6, 22),
                    SubdivisionId = _context.Subdivisions.FirstOrDefault().Id,
                    DocumentId = _context.Documents.FirstOrDefault().Id,
                    DocumentPage = 10,
                    LocationId = _context.Locations.FirstOrDefault().Id,
                };
                ActualData secondActualData = new ActualData()
                {
                    Date = new DateTime(1941, 6, 23),
                    SubdivisionId = _context.Subdivisions.FirstOrDefault().Id,
                    DocumentId = _context.Documents.FirstOrDefault().Id,
                    DocumentPage = 18,
                    LocationId = _context.Locations.OrderBy(ad => ad.Id).LastOrDefault().Id
                };
                _context.ActualData.AddRange(firstActualData, secondActualData);
                _context.SaveChanges(); 
            }
        }

        public Task<List<ActualData>> GetActualData()
        {
            var actualData = _context.ActualData
                .Include(actualData => actualData.Subdivision)
                .ThenInclude(s => s.Commander)
                .ThenInclude(c => c.Rank)
                .Include(actualData => actualData.Subdivision)
                .ThenInclude(s => s.TypeOfSubdivision)
                .Include(actualData => actualData.Location)
                .Include(actualData => actualData.Document)
                .ToListAsync();
            return actualData;
        }

        public Task<ActualData> GetActualDataById(int id)
        {
            return Task.Run(()=>_context.ActualData.Find(id));
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var actualData = GetActualDataById(id).Result;
            if (actualData == null)
            {
                await Task.Run(()=> false);
            } 
            _context.ActualData.Remove(actualData);
            await _context.SaveChangesAsync();
            return await Task.Run(()=>true);
        }
    }
}