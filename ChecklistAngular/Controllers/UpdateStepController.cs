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
    public class UpdateStepController : ControllerBase
    {
        
        private readonly IUpdateRepository _repo;

        public UpdateStepController(IUpdateRepository repo)
        {
            
           _repo = repo;
        }

        [HttpPost("{updateId}/{stepId}")]
        public async Task<ActionResult> ChecklistAction(int updateId,int stepId, [FromQuery] string action)
        {
            var step = await _repo.GetUpdateSteps(stepId, updateId);
            var status = _repo.GetStatus(updateId);
            if (step == null)
                BadRequest("Step does not exist");
            switch(action)
            {
                case "complete":
                    step.Progress = "Done";
                    LogHistory(step,status, "Completed Step ");
                    break;
                case "skip":
                    step.Progress = "Skip";
                    LogHistory(step, status, "Skipped Step ");
                    break;
                case "clear":
                    step.Progress = "";
                    LogHistory(step, status, "Unchecked Step ");
                    break;
            }
            if(await _repo.SaveAll())
             return NoContent();
            return BadRequest("Error Saving, Please Try again");
        }

        [HttpPost("{updateId}/{stepId}/comment")]
        public async Task<ActionResult> SaveComment(int updateId, int stepId, string comment)
        {
            var step = await _repo.GetUpdateSteps(stepId, updateId);
            step.Comment = comment;
            if(await _repo.SaveAll())
            return NoContent();
            return BadRequest("Error Saving the comment please try again");
        }

        private void LogHistory(LogUpdateSteps step, string status, string action)
        {
            var user = WindowsIdentity.GetCurrent().Name;
            var history = new LogUpdateHistory
            {
                Idupdate = step.Idupdate,
                FileTime = DateTime.Now,
                FileBy = user.Substring(user.IndexOf(@"\") + 1),
                Status = status,
                FileAction = action + step.Step
            };

            _repo.Add(history);

        }

    }
}