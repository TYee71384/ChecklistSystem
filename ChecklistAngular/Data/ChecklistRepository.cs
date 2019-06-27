using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChecklistAngular.Helpers;
using ChecklistAngular.Models;
using Microsoft.EntityFrameworkCore;

namespace ChecklistAngular.Data
{
    public class ChecklistRepository : IChecklistRepository
    {
        private readonly SWAT_UpdateChecklistsContext _ctx;

        public ChecklistRepository(SWAT_UpdateChecklistsContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<LogChecklist> GetChecklist(int id, int ver)
        {
            var checklist = new LogChecklist();
            if (ver > 0)
            {
                checklist = await _ctx.LogChecklist
                          .Include(x => x.LogChecklistSteps)
                           .Include(x => x.LogChecklistHistory)
                           .Where(x => x.Idchecklist == id && x.Version == ver).FirstOrDefaultAsync();
                
            }
            else
            {
                checklist = await _ctx.LogChecklist
                               .Include(x => x.LogChecklistSteps)
                                .Include(x => x.LogChecklistHistory)
                                .Where(x => x.Idchecklist == id && x.Version >= ver).OrderByDescending(v => v.Version).FirstOrDefaultAsync();
            }
            checklist.LogChecklistSteps = checklist.LogChecklistSteps.OrderBy(x => x.Step).ToList();
            return checklist;
        }

        //check if draft exists
        public async Task<LogChecklist> CheckForDraft(int id)
        {
            return await _ctx.LogChecklist.FirstOrDefaultAsync(x => x.Idchecklist == id && x.Status == "Draft");
        }

        public async Task<LogChecklistIndex> GetIndex(int id)
        {
            return await _ctx.LogChecklistIndex.FindAsync(id);
        }

        public async Task<IEnumerable<LogChecklist>> GetChecklists()
        {
            return await _ctx.LogChecklist.ToListAsync();
        }

        //server side paging (commented out in case ned in the future, using client side instead for use with primeng datatable)
        //public async Task<PagedList<LogChecklist>> GetChecklists(ChecklistParams cparams)
        //{
        //    var checklists = _ctx.LogChecklist.AsQueryable();

        //    if (cparams.Platform != null)
        //    {
        //        checklists = checklists.Where(c => c.ProdLine == cparams.Platform);
        //    }
        //    if (cparams.System != null)
        //    {
        //        checklists = checklists.Where(c => c.System == cparams.System);
        //    }
        //    if (cparams.Process != null)
        //    {
        //        checklists = checklists.Where(c => c.Process == cparams.Process);
        //    }
        //    if (cparams.Keyword != null)
        //    {
        //        checklists = checklists.Where(c => c.Title.Contains(cparams.Keyword.ToLower()));
        //    }

        //    return await PagedList<LogChecklist>.CreateAsync(checklists, cparams.PageNumber, cparams.PageSize);
        //}

       

       

    

        public async Task<LogChecklist> DraftExists(int id)
        {
            return await _ctx.LogChecklist.FirstOrDefaultAsync(x => x.Idchecklist == id && x.Status == "Draft");
        }

        public async Task<IEnumerable<LogChecklistSteps>> GetSteps(int id, short ver)
        {
            return await _ctx.LogChecklistSteps.Where(x => x.Idchecklist == id && x.Version == ver).ToListAsync();
        }

        

        public void Add<T>(T entity) where T : class
        {
            _ctx.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _ctx.Remove(entity);
        }

        public async Task<bool> SaveAll()
        {
            return await _ctx.SaveChangesAsync() > 0;
        }

        public void EditChecklist(LogChecklist checklist) 
        {
            _ctx.Entry(checklist).State = EntityState.Modified;
        }

        public async Task<LogChecklist> GetApprovedChecklist(int id, short previous)
        {
           return await _ctx.LogChecklist.FirstOrDefaultAsync(x => x.Idchecklist == id && x.Version == previous && x.Status == "Approved");
        }

        public async Task<LogChecklistHistory> GetApprovedDraft(int id, int ver)
        {
           return await _ctx.LogChecklistHistory.Where(x => x.Idchecklist == id && x.Version == ver && x.Status == "Draft").OrderByDescending(x => x.FileTime).FirstAsync();
        }

        public int GetStepCount(LogChecklistSteps step)
        {
           return _ctx.LogChecklistSteps.Where(x => x.Idchecklist == step.Idchecklist && x.Version == step.Version).Count() + 1;
        }

        public async Task<LogChecklistSteps> GetChecklistStep(int stepId)
        {
            return await _ctx.LogChecklistSteps.FirstOrDefaultAsync(x => x.Idstep == stepId);
        }


        public void EditStep(LogChecklistSteps step )
        {
            _ctx.Entry(step).State = EntityState.Modified;
        }

        public void ReorderSteps(LogChecklistSteps step)
        {
            //_ctx.LogChecklistSteps.Update(step);
            _ctx.Attach(step);
            _ctx.Entry(step).Property("Step").IsModified = true;
        }


      
    }
}
