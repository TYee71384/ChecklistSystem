using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ChecklistAngular.Models
{
    public partial class LogChecklist
    {
        public LogChecklist()
        {
            LogChecklistHistory = new HashSet<LogChecklistHistory>();
            LogChecklistSteps = new HashSet<LogChecklistSteps>();
        }

        public int Idchecklist { get; set; }
        public short Version { get; set; }
        public string Status { get; set; }
        public string Title { get; set; }
        public string ProdLine { get; set; }
        public string System { get; set; }
        public string Process { get; set; }
        public string Rel { get; set; }
        public string Type { get; set; }
        public string Scope { get; set; }

        [JsonIgnore]
        public LogChecklistIndex IdchecklistNavigation { get; set; }
        public ICollection<LogChecklistHistory> LogChecklistHistory { get; set; }
        public ICollection<LogChecklistSteps> LogChecklistSteps { get; set; }
    }
}
