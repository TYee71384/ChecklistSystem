using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using ChecklistAngular.Data;
using ChecklistAngular.Helpers;
using ChecklistAngular.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChecklistAngular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateController : ControllerBase
    {
        private readonly IChecklistRepository _repo;

        public UpdateController(IChecklistRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult> GetUpdateChecklists()
        {
           var returnList = await _repo.GetUpdates();

          //  Response.AddPagination(returnList.CurrentPage, returnList.PageSize, returnList.TotalCount, returnList.TotalPages);

            return Ok(returnList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LogUpdate>> GetUpdate(int id)
        {
            var checklist = await _repo.GetUpdate(id);
            if (checklist == null)
                return BadRequest("Update does not exits");
            
            return checklist;
        }

        [HttpGet("{id}/Progress")]
        public async Task<ActionResult> GetProgress(int id)
        {
            var checklist =  await _repo.GetUpdate(id);
            if (checklist == null)
                return BadRequest("Update does not exits");
            var progress = Helpers.Helpers.GetPercentage(checklist);
            return Ok(progress);
        }

        [HttpGet("select")]
        public async Task<ActionResult> ChecklistSelect([FromQuery] UpdateParams uparams)
        {
            if (uparams.Platform == null)
                return BadRequest("Platform is missing");
            if (uparams.Site == null)
                return BadRequest("Site is missing");
            if (uparams.UpdateNum < 0)
                return BadRequest("Update number is missing");
            if (uparams.System == null)
                return BadRequest("System is missing");
            if (uparams.Process == null)
                return BadRequest("Process is missing");

            var checklist = await _repo.SingleChecklistSelect(uparams);
            if (checklist != null)
                return Ok(checklist);

            var checklists = await _repo.ChecklistSelect(uparams); 
                return Ok(checklists);
        }

        [HttpPost("{id}/admin")]
        [Authorize(Policy = "AccessUser")]
        public async Task<ActionResult> AdminComplete(int id)
        {
            var update = await _repo.GetUpdate(id);
            update.EndTime = DateTime.Now;
            update.Status = "Complete";
            LogHistory(id, "Complete", "Admin Complete");
           if(await _repo.SaveAll())
            return NoContent();
            return BadRequest("Cannot Complete. Please Try Again");
           
        }

        private void LogHistory(int id, string status, string action)
        {
            var user = WindowsIdentity.GetCurrent().Name;
            var history = new LogUpdateHistory
            {
                Idupdate = id,
                FileTime = DateTime.Now,
                FileBy = user.Substring(user.IndexOf(@"\") + 1),
                Status = status,
                FileAction = action
            };

            _repo.Add(history);

        }





    }
}