using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChecklistAngular.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChecklistAngular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DictionaryController : ControllerBase
    {
        private readonly SWAT_UpdateChecklistsContext _ctx;

        public DictionaryController(SWAT_UpdateChecklistsContext ctx)
        {
            _ctx = ctx;
        }

        public ActionResult<DropDownOptions> GetDictionaries()
        {
            var dictionary =  new DropDownOptions()
            {
                PPacks = _ctx.ListPpack.Select(x => x.Ppack),
                Processes = _ctx.ListProcess.Select(x => x.Process),
                ProdLines = _ctx.ListProdLine.Select(x => x.ProdLine),
                Releases = _ctx.ListRelease.Select(x => x.Rel),
                Systems = _ctx.ListSystem.Select(x => x.System),
                Types = _ctx.ListType.Select(x => x.Type)
               
            };

            return  dictionary;
        }

        [HttpGet("site/{platform}")]
        public async Task<IEnumerable<ListSite>> GetSiteByPlatform(string platform)
        {
            return await _ctx.ListSite.Where(x => x.ProdLine == platform).ToListAsync();
            
        }

        [HttpPost("site/{platform}")]
        public async Task<ActionResult> AddNewSite(ListSite site)
        {
             _ctx.ListSite.Add(site);
            await _ctx.SaveChangesAsync();
            return NoContent();
        }
       
    }

    public class DropDownOptions
    {
        public IEnumerable<string> PPacks { get; set; }
        public IEnumerable<string> Processes { get; set; }
        public IEnumerable<string> ProdLines { get; set; }
        public IEnumerable<string> Releases { get; set; }
        public IEnumerable<string> Systems { get; set; }
        public IEnumerable<string> Types { get; set; }

    }
}


