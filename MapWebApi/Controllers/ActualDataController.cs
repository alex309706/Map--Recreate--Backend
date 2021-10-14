using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MapWebApi.Models;
using MapWebApi.Services.Interfaces;
using MapWebApi.ViewModels;

namespace MapWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActualDataController : ControllerBase
    {
        private readonly SubdivisionsContext _context;
        private IActualDataService _actualDataService;

        public ActualDataController(SubdivisionsContext context,IActualDataService actualDataService)
        {
            _context = context;
            _actualDataService = actualDataService;
            _actualDataService.SeedData();
           
        }
        // получение коллекции данных за всю историю: api/ActualData
        [HttpGet]
        public async Task<List<ActualData>> GetActualData()
        {
            return await _actualDataService.GetActualData();
        }
        // получение коллекции элементов на определенную дату: api/ActualData/date
        [HttpGet("{date}")]
        public async Task<ActionResult<IEnumerable<ActualData>>> GetActualData(DateTime date)
        {
            return await _context.ActualData.Where(actualData=>actualData.Date == date)
                .Include(actualData => actualData.Subdivision)
                .ThenInclude(s => s.Commander)
                .ThenInclude(c => c.Rank)
                .Include(actualData => actualData.Subdivision)
                .ThenInclude(s => s.TypeOfSubdivision)
                .Include(actualData => actualData.Location)
                .Include(actualData => actualData.Document)
                .ToListAsync();
        }
        //Изменение элемента
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActualData(int id, ActualData actualData)
        {
            if (id != actualData.Id)
            {
                return BadRequest();
            }

            _context.Entry(actualData).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActualDataExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        // создание элемента: api/ActualData
        [HttpPost]
        public async Task<ActionResult<ActualData>> PostActualData(ActualDataViewModel inputActualData)
        {
            ActualData newActualData = new ActualData
            {
                Date = inputActualData.Date,
                SubdivisionId = inputActualData.SubdivisionId,
                LocationId = inputActualData.LocationId,
                DocumentId = inputActualData.DocumentId,
                DocumentPage = inputActualData.DocumentPage
            };

            _context.ActualData.Add(newActualData);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetActualData", new { id = newActualData.Id }, newActualData);
        }
        // удаление элемента: api/ActualData/5
        [HttpDelete("{id}")]
        public async Task<bool> DeleteActualData(int id)
        {
            return await _actualDataService.DeleteAsync(id);
        }

        private bool ActualDataExists(int id)
        {
            return _context.ActualData.Any(e => e.Id == id);
        }
    }
}
