using MapWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapWebApi.ViewModels;


namespace MapWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandersController : ControllerBase
    {
        SubdivisionsContext db;
        public CommandersController(SubdivisionsContext context)
        {
            db = context;
            //инициализация, если нет данных
            if (!db.Commanders.Any())
            {
                Rank LieutenantGeneral = db.Ranks.FirstOrDefault(x => x.Name == "Генерал-Лейтенант");
                Rank MajorGeneral = db.Ranks.FirstOrDefault(x => x.Name == "Генерал-Майор");
                db.Commanders.Add(new Commander { LastName = "Кузнецов", Rank = LieutenantGeneral });
                db.Commanders.Add(new Commander { LastName = "Коробков", Rank = MajorGeneral });
                db.SaveChanges();
            }
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Commander>>> Get()
        {
           
            return await db.Commanders.Include(c=>c.Rank).ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Commander>> Get(int id)
        {
           
            Commander commander = await db.Commanders.FirstOrDefaultAsync(x => x.Id == id);
            if (commander == null)
                return NotFound();
            return new ObjectResult(commander);
        }
        [HttpPost]
        public async Task<ActionResult<Commander>> Post(CommanderViewModel commander)
        {
            
            if (commander == null)
            {
                return BadRequest();
            }
            Commander newCommander = new Commander {LastName = commander.LastName, RankId = commander.RankId };

            db.Commanders.Add(newCommander);
            await db.SaveChangesAsync();
            return Ok(newCommander);
        }

        [HttpPut]
        public async Task<ActionResult<Commander>> Put(Commander commander)
        {
            if (commander == null)
            {
                return BadRequest();
            }
            if (!db.Commanders.Any(x => x.Id == commander.Id))
            {
                return NotFound();
            }

            db.Update(commander);
            await db.SaveChangesAsync();
            return Ok(commander);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Commander>> Delete(int id)
        {
            Commander commander= db.Commanders.FirstOrDefault(x => x.Id == id);
            if (commander == null)
            {
                return NotFound();
            }
            db.Commanders.Remove(commander);
            await db.SaveChangesAsync();
            return Ok(commander);
        }


    }
}
