using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using AutoMapper;
using ChecklistAngular.Data;
using ChecklistAngular.DTOs;
using ChecklistAngular.Helpers;
using ChecklistAngular.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChecklistAngular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChecklistController : ControllerBase
    {

        private readonly IChecklistRepository _repo;
        private readonly IMapper _mapper;
        private readonly string user;
        public ChecklistController(IChecklistRepository repo, IMapper mapper)
        {

            _repo = repo;
            _mapper = mapper;
            user = WindowsIdentity.GetCurrent().Name;
            user = user.Substring(user.IndexOf(@"\") + 1);
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] ChecklistParams cparams)
        {

            var returnList = await _repo.GetChecklists(cparams);
            Response.AddPagination(returnList.CurrentPage, returnList.PageSize, returnList.TotalCount, returnList.TotalPages);

            return Ok(returnList);

        }

        [HttpGet("{id}/{ver?}", Name = "GetById")]
        public async Task<ActionResult<LogChecklist>> GetbyId([FromRoute] int id, int ver)
        {
            var checklist = await _repo.GetChecklist(id, ver);
            return checklist;
        }

        [HttpPost]
        public async Task<ActionResult> CreateChecklist([FromBody] LogChecklist checklist)
        {

            checklist.Version = 1;
            checklist.Status = "Draft";

            var index = new LogChecklistIndex()
            {
                CreatedBy = user,
                CreatedTime = DateTime.Now,
            };
            _repo.Add(index);
            checklist.Idchecklist = index.Idchecklist;
            _repo.Add(checklist);

            var history = LogHistory(checklist, "Create new Checklist Version", 1);
            _repo.Add(history);


            if (await _repo.SaveAll())

                return CreatedAtAction("GetbyId", new { id = checklist.Idchecklist, ver = checklist.Version }, checklist);

            return BadRequest("Failed to Create Checklist");
        }

        [HttpPut("{id}/{ver}")]
        public async Task<IActionResult> EditChecklist(int id, int ver, ChecklistDescription checklistDescription)
        {
            //Edit the description of checklist to edit steps use ChecklistStepsController
            //var user = WindowsIdentity.GetCurrent().Name;
            var checklist = _mapper.Map<LogChecklist>(checklistDescription);
            var history = LogHistory(checklist, "Edited Description", (short)ver);
            _repo.Add(history);

            _repo.EditChecklist(checklist);
            if (await _repo.SaveAll())

                return NoContent();

            return BadRequest("Failed to Edit checklist");
        }

        [HttpPost("{id}/{ver}/archive")]
        public async Task<ActionResult> ArchiveChecklist(int id, int ver)
        {
            var checklist = await _repo.GetChecklist(id, ver);
            var status = checklist.Status;
            
            if(status == "Approved")
            {
              var draft = await _repo.DraftExists(id);
                if (draft != null)
                    return BadRequest("Cannot archive when there is a Draft version");
            }else
            {
                return BadRequest("Invalid Status");
            }

            checklist.Status = "Archived";
            _repo.EditChecklist(checklist);
          var history =  LogHistory(checklist, "Marked as Archived", (short)ver);
            _repo.Add(history);
            
           await _repo.SaveAll();
            return NoContent();



        }

        [HttpPost("{id}/{ver}/approve")]
        public async Task<ActionResult> ApproveChecklist(int id, int ver)
        {
            var checklist = await _repo.GetChecklist(id, ver);

            var draft = await _repo.GetApprovedDraft(id,ver);
            var lastEditBy = draft.FileBy;
            var lastEditTime = draft.FileTime;
           

            //make sure checklist is in Draft Status
            if (checklist.Status != "Draft")
                return BadRequest("Checklist is not in Draft Status");

            //make sure user is not the last edited user
            if (user == lastEditBy)
                return BadRequest("You were the last to edit this checklist");

            //set previous approved to Archived
            if (checklist.Version > 1)
            {
                var previous = checklist.Version - 1;
                var archived = await _repo.GetApprovedChecklist(id, (short)previous);
                archived.Status = "Archived";
                var history = LogHistory(archived, "Marked As Archived", (short)previous);
                _repo.Add(history);
            }
            //set currnet Draft version to Approved
            checklist.Status = "Approved";
            var Approvehistory = LogHistory(checklist, "Marked as Approved", checklist.Version);
            _repo.Add(Approvehistory);
            await _repo.SaveAll();
            //TODO:: send email

            return NoContent();
        }

        [HttpDelete("{id}/{ver}")]
        public async Task<ActionResult> DeleteChecklist(int id, short ver)
        {
            var checklist = await _repo.GetChecklist(id, ver);
            var status = checklist.Status;
            

            if(status == "Draft")
            {
                _repo.Delete(checklist);
                _repo.Delete(await _repo.GetIndex(id));
            }

            if (await _repo.SaveAll())
                return NoContent();
            return BadRequest("Problem deleting checklist please try again");

        }

        public LogChecklistHistory LogHistory(LogChecklist checklist, string action, short ver)
        {

            return new LogChecklistHistory
            {
                Idchecklist = checklist.Idchecklist,
                Version = ver,
                Status = checklist.Status,
                FileTime = DateTime.Now,
                FileAction = action,
                FileBy = user

            }; 
            
        }


    }
}