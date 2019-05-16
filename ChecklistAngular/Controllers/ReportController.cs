using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ChecklistAngular.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChecklistAngular.Helpers;

namespace ChecklistAngular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly SWAT_UpdateChecklistsContext _ctx;
        private readonly IMapper _mapper;

        public ReportController(SWAT_UpdateChecklistsContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> GetStatusReport()
        {
            var update= await _ctx.LogUpdate
                .Include(x => x.LogUpdateSteps)
                .Where(x => x.Process == "Prerequisite" && x.StartTime >= DateTime.Now.AddYears(-1)).ToListAsync();
            var report = new  List<Report>();
            foreach (var i in update)
            {
                var r = _mapper.Map<Report>(i);
                var p = Helpers.Helpers.GetPercentage(i);
                r.Percentage = p;
                report.Add(r);
            }
            return Ok(report);
        }
    }
}