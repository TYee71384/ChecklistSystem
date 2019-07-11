using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using ChecklistAngular.Data;
using ChecklistAngular.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChecklistAngular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChecklistStepsController : ControllerBase
    {

        private readonly IChecklistRepository _repo;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly string user;


        public ChecklistStepsController(IChecklistRepository repo, IHttpContextAccessor httpContextAccessor)
        {

            _repo = repo;
            this.httpContextAccessor = httpContextAccessor;
            user = this.httpContextAccessor.HttpContext.User.Identity.Name;
            // user = User.Identity.Name;
            // user = WindowsIdentity.GetCurrent().Name;
            user = user.Substring(user.IndexOf(@"\") + 1);
        }

        [HttpGet("{stepId}", Name = "GetStep")]
        public async Task<ActionResult> GetStep(int checklistId, int checklistVer, int stepId)
        {
            var step = await _repo.GetChecklistStep(stepId);
            return Ok(step);
        }


        [HttpPost]
        public async Task<ActionResult> AddStep(LogChecklistSteps step)
        {

            //get count
            var stepCount = _repo.GetStepCount(step);

            step.Step = (short)stepCount;

            _repo.Add(step);
            var history = FileHistory(step, "Draft", "Added a Step");


            _repo.Add(history);

            await _repo.SaveAll();

            return CreatedAtRoute("GetStep", new { stepId = step.Idstep }, step);
        }

        [HttpPut("{stepId}")]
        public async Task<ActionResult> EditStep(int stepId, LogChecklistSteps step)
        {

            step.Idstep = stepId;
            _repo.EditStep(step);
            var history = FileHistory(step, "Draft", "Edited a Step");

            _repo.Add(history);
            await _repo.SaveAll();
            return NoContent();
        }

        [HttpPost("{stepId}")]
        public async Task<ActionResult> DeleteStep(int stepId, LogChecklistSteps step)
        {
           _repo.Delete(step);
            var checklist = await _repo.GetChecklist(step.Idchecklist, step.Version);
            var stepNum = 1;
          var steps =  checklist.LogChecklistSteps.OrderByDescending(s => s.Step);
            foreach (var s in steps)
            {
                s.Step = (short)stepNum;
                stepNum++;
            }
            _repo.Add(FileHistory(step, "Draft", "Deleted a Step"));

            if(await _repo.SaveAll())
            return NoContent();
            return BadRequest("There was a problem deleting this step. Please try again");
        }

        [HttpPut("reorder")]
        public async Task<ActionResult> ReorderSteps(LogChecklist checklist)
        {
            
            foreach(var s in checklist.LogChecklistSteps)
            {
                _repo.ReorderSteps(s);
            }
           
            await _repo.SaveAll();
            return NoContent();
        }

        private LogChecklistHistory FileHistory(LogChecklistSteps step, string status, string action)
        {
            return new LogChecklistHistory
            {
                Idchecklist = step.Idchecklist,
                Version = step.Version,
                Status = status,
                FileTime = DateTime.Now,
                FileAction = action,
                FileBy = user
            };
        }
        
    }
}