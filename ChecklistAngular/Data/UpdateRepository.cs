using ChecklistAngular.Helpers;
using ChecklistAngular.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChecklistAngular.Data
{
    public class UpdateRepository : IUpdateRepository
    {
        private readonly SWAT_UpdateChecklistsContext _ctx;

        public UpdateRepository(SWAT_UpdateChecklistsContext ctx)
        {
            _ctx = ctx;
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

        public Task<LogUpdate> GetUpdate(int id)
        {
            var checklist = _ctx.LogUpdate.Include(x => x.LogUpdateSteps).Include(x => x.LogUpdateHistory).FirstOrDefaultAsync(x => x.Idupdate == id);
            return checklist;
        }
        //  return await _ctx.LogUpdate.Include(x => x.LogUpdateSteps).Include(x => x.LogUpdateHistory).FirstOrDefaultAsync(x => x.Idupdate == id);


        public async Task<IEnumerable<LogUpdate>> GetUpdates()
        {
            return await _ctx.LogUpdate.ToListAsync();
        }

       
        //server side paging (commented out in case ned in the future, using client side instead for use with primeng datatable)
        //public async Task<PagedList<LogUpdate>> GetUpdates(UpdateParams uparams)
        //{
        //    var updateChecklists = _ctx.LogUpdate.AsQueryable();

        //    if (uparams.Platform != null)
        //        updateChecklists = updateChecklists.Where(u => u.ProdLine == uparams.Platform);
        //    if (uparams.System != null)
        //        updateChecklists = updateChecklists.Where(u => u.System == uparams.Platform);
        //    if (uparams.Process != null)
        //        updateChecklists = updateChecklists.Where(u => u.Process == uparams.Process);
        //    if (uparams.Release != null)
        //        updateChecklists = updateChecklists.Where(u => u.UpdateRelease == uparams.Release);
        //    if (uparams.Task > 0)
        //        updateChecklists = updateChecklists.Where(u => u.Task == uparams.Task);

        //    return await PagedList<LogUpdate>.CreateAsync(updateChecklists, uparams.PageNumber, uparams.PageSize);

        //}

        public async Task<LogUpdate> SingleChecklistSelect(UpdateParams uparams)
        {
            return await _ctx.LogUpdate.Where(x => x.ProdLine == uparams.Platform && x.SiteKml == uparams.Site && x.UpdateNum == uparams.UpdateNum && x.System == uparams.System && x.Process == uparams.Process).FirstOrDefaultAsync();
        }
        public async Task<LogUpdateSteps> GetUpdateSteps(int stepId, int updateId)
        {
            return await _ctx.LogUpdateSteps.FirstOrDefaultAsync(x => x.Idupdate == updateId && x.Step == stepId);
        }
        public string GetStatus(int updateId)
        {
            return _ctx.LogUpdate.Where(x => x.Idupdate == updateId).Select(x => x.Status).Single();
        }
        public async Task<LogUpdate> UpdateExists(LogUpdate update)
        {
            var updateExist = await _ctx.LogUpdate.Where(x => x.ProdLine == update.ProdLine && x.SiteKml == update.SiteKml && x.UpdateNum == update.UpdateNum && x.System == update.System && x.Process == update.Process && x.Idchecklist == update.Idchecklist && x.Status != "Cancelled").FirstOrDefaultAsync();
            return updateExist;
        }
        public async Task<IEnumerable<LogChecklistSteps>> GetSteps(int id, short ver)
        {
            return await _ctx.LogChecklistSteps.Where(x => x.Idchecklist == id && x.Version == ver).ToListAsync();
        }
        public async Task<IEnumerable<LogChecklist>> ChecklistSelect(UpdateParams uparams)
        {
            return await _ctx.LogChecklist.Where(x => x.ProdLine == uparams.Platform && x.System == uparams.System && x.Process == uparams.Process && x.Status == "Approved").OrderBy(x => x.Version).ToListAsync();
        }


    }
}
