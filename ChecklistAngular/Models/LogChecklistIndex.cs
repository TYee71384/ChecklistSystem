using System;
using System.Collections.Generic;

namespace ChecklistAngular.Models
{
    public partial class LogChecklistIndex
    {
        public LogChecklistIndex()
        {
            LogChecklist = new HashSet<LogChecklist>();
            LogChecklistHistory = new HashSet<LogChecklistHistory>();
            LogChecklistSteps = new HashSet<LogChecklistSteps>();
        }

        public int Idchecklist { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedTime { get; set; }

        public ICollection<LogChecklist> LogChecklist { get; set; }
        public ICollection<LogChecklistHistory> LogChecklistHistory { get; set; }
        public ICollection<LogChecklistSteps> LogChecklistSteps { get; set; }
    }
}
