using ChecklistAngular.Helpers;
using ChecklistAngular.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChecklistAngular.Data
{
   public interface IChecklistRepository
    {
       // Task<PagedList<LogChecklist>> GetChecklists(ChecklistParams cparams);
        Task<LogChecklist> GetChecklist(int id, int ver);
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();
        void EditChecklist(LogChecklist checklist);
        Task<LogChecklist> GetApprovedChecklist(int id, short previous );
        Task<LogChecklistHistory> GetApprovedDraft(int id, int ver);
        int GetStepCount(LogChecklistSteps step);
        Task<LogChecklistSteps> GetChecklistStep(int stepId);
        void EditStep(LogChecklistSteps step);
        Task<LogChecklist> DraftExists(int id);
        Task<LogChecklistIndex> GetIndex(int id);
        Task<IEnumerable<LogChecklist>> GetChecklists();
        void ReorderSteps(LogChecklistSteps step);
        Task<LogChecklist> CheckForDraft(int id);
        Task<IEnumerable<LogChecklistSteps>> GetSteps(int id, short ver);
        
    }
}
