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
            return checklist;
        }

        public async Task<PagedList<LogChecklist>> GetChecklists(ChecklistParams cparams)
        {
            var checklists = _ctx.LogChecklist.AsQueryable();

            if (cparams.Platform != null)
            {
                checklists = checklists.Where(c => c.ProdLine == cparams.Platform);
            }
            if (cparams.System != null)
            {
                checklists = checklists.Where(c => c.System == cparams.System);
            }
            if (cparams.Process != null)
            {
                checklists = checklists.Where(c => c.Process == cparams.Process);
            }
            if (cparams.Keyword != null)
            {
                checklists = checklists.Where(c => c.Title.Contains(cparams.Keyword.ToLower()));
            }

            return await PagedList<LogChecklist>.CreateAsync(checklists, cparams.PageNumber, cparams.PageSize);
        }

        public async Task<LogUpdate> GetUpdate(int id)
        {
            return await _ctx.LogUpdate.Include(x => x.LogUpdateSteps).Include(x => x.LogUpdateHistory).FirstOrDefaultAsync(x => x.Idupdate == id);
        }

        public async Task<PagedList<LogUpdate>> GetUpdates(UpdateParams uparams)
        {
            var updateChecklists = _ctx.LogUpdate.AsQueryable();

            if (uparams.Platform != null)
                updateChecklists = updateChecklists.Where(u => u.ProdLine == uparams.Platform);
            if (uparams.System != null)
                updateChecklists = updateChecklists.Where(u => u.System == uparams.Platform);
            if (uparams.Process != null)
                updateChecklists = updateChecklists.Where(u => u.Process == uparams.Process);
            if (uparams.Release != null)
                updateChecklists = updateChecklists.Where(u => u.UpdateRelease == uparams.Release);
            if (uparams.Task > 0)
                updateChecklists = updateChecklists.Where(u => u.Task == uparams.Task);

            return await PagedList<LogUpdate>.CreateAsync(updateChecklists, uparams.PageNumber, uparams.PageSize);

        }

        public async Task<LogUpdate> SingleChecklistSelect(UpdateParams uparams)
        {


            return await _ctx.LogUpdate.Where(x => x.ProdLine == uparams.Platform && x.SiteKml == uparams.Site && x.UpdateNum == uparams.UpdateNum && x.System == uparams.System && x.Process == uparams.Process).FirstOrDefaultAsync();
           
            
        }

        public async Task<IEnumerable<LogChecklist>> ChecklistSelect(UpdateParams uparams)
        {
           

                return await _ctx.LogChecklist.Where(x => x.ProdLine == uparams.Platform && x.System == uparams.System && x.Process == uparams.Process && x.Status == "Approved").OrderBy(x => x.Version).ToListAsync();
            
        }

        public async Task<LogChecklist> DraftExists(int id)
        {
            return await _ctx.LogChecklist.FirstOrDefaultAsync(x => x.Idchecklist == id && x.Status == "Draft");
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

        public async Task<LogUpdateSteps> GetUpdateSteps(int stepId, int updateId)
        {
           return await _ctx.LogUpdateSteps.FirstOrDefaultAsync(x => x.Idupdate == updateId && x.Step == stepId);
        }

        public void EditStep(LogChecklistSteps step )
        {
            _ctx.Entry(step).State = EntityState.Modified;
        }

        public string GetStatus(int updateId)
        {
           return _ctx.LogUpdate.Where(x => x.Idupdate == updateId).Select(x => x.Status).Single();
        }
    }
}
