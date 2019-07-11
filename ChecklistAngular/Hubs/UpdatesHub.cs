using ChecklistAngular.Data;
using ChecklistAngular.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace ChecklistAngular.Hubs
{
    public class UpdatesHub : Hub
    {
        private readonly IUpdateRepository _repo;
        private readonly IHttpContextAccessor httpContextAccessor;

        public UpdatesHub(IUpdateRepository repo, IHttpContextAccessor httpContextAccessor)
        {
            _repo = repo;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task GetProgress(int id, int stepNum, string action)
        {
            var step = await _repo.GetUpdateSteps(stepNum, id);
            var updatechecklist = await _repo.GetUpdate(step.Idupdate);
            step.Progress = action;
            switch (action)
            {
                case "Done":
                    
                    LogHistory(step, updatechecklist.Status, "Completed Step ");
                    break;
                case "Skip":
                    step.Progress = "Skip";
                    LogHistory(step, updatechecklist.Status, "Skipped Step ");
                    break;
                case "":
                    step.Progress = "";
                    LogHistory(step, updatechecklist.Status, "Unchecked Step ");
                    break;
            }
            await _repo.SaveAll();
            var percentage = Helpers.Helpers.GetPercentage(updatechecklist);
            await Clients.All.SendAsync("StepProgress", step.Progress, step.Step, step.Comment, percentage);
        }

        public async Task SaveComment(int id, int stepNum, string comment)
        {
            var step = await _repo.GetUpdateSteps(stepNum, id);
            step.Comment = comment;
            await _repo.SaveAll();
            await Clients.All.SendAsync("StepProgress", step.Progress, step.Step, step.Comment);

        }

        private void LogHistory(LogUpdateSteps step, string status, string action)
        {
         
           var user = this.httpContextAccessor.HttpContext.User.Identity.Name;
            
            user = user.Substring(user.IndexOf(@"\") + 1);
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
