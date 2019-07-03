using ChecklistAngular.Data;
using ChecklistAngular.Models;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var update = await _repo.GetUpdate(id);
            var step = await _repo.GetUpdateSteps(stepNum, id);
            step.Progress = action;
            await _repo.SaveAll();
            var json = JsonConvert.SerializeObject(update);
            await Clients.All.SendAsync("StepProgress", json);
        }

        
    }
}
