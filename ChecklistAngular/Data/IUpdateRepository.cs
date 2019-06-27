using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChecklistAngular.Data
{
    public interface IUpdateRepository
    {
        void Add<T>(T entity) where T : class;
        Task<IEnumerable<Models.LogChecklist>> ChecklistSelect(Helpers.UpdateParams uparams);
        void Delete<T>(T entity) where T : class;
        string GetStatus(int updateId);
        Task<IEnumerable<Models.LogChecklistSteps>> GetSteps(int id, short ver);
        Task<Models.LogUpdate> GetUpdate(int id);
        Task<IEnumerable<Models.LogUpdate>> GetUpdates();
        Task<Models.LogUpdateSteps> GetUpdateSteps(int stepId, int updateId);
        Task<bool> SaveAll();
        Task<Models.LogUpdate> SingleChecklistSelect(Helpers.UpdateParams uparams);
        Task<Models.LogUpdate> UpdateExists(Models.LogUpdate update);
    }
}
