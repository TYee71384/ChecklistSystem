using ChecklistAngular.Data;
using ChecklistAngular.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChecklistAngular.Hubs
{
    public class UpdatesHub : Hub
    {
        private readonly IUpdateRepository _repo;

        public async override Task OnConnectedAsync()
        {
            
        }

        public UpdatesHub(IUpdateRepository repo)
        {
            _repo = repo;
        }

        
    }
}
