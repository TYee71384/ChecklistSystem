using ChecklistAngular.Data;
using ChecklistAngular.Models;
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

       

        public UpdatesHub(IUpdateRepository repo)
        {
            _repo = repo;
        }

        public async Task GetProgress(int id, int stepNum, string action)
        {
            var step = await _repo.GetUpdateSteps(stepNum, id);
            var status = await _repo.GetUpdate(step.Idupdate);
            step.Progress = action;
            switch (action)
            {
                case "Done":
                    
                    LogHistory(step, status.Status, "Completed Step ");
                    break;
                case "Skip":
                    step.Progress = "Skip";
                    LogHistory(step, status.Status, "Skipped Step ");
                    break;
                case "":
                    step.Progress = "";
                    LogHistory(step, status.Status, "Unchecked Step ");
                    break;
            }
            await _repo.SaveAll();
            await Clients.All.SendAsync("StepProgress", step.Progress, step.Step, step.Comment);
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
