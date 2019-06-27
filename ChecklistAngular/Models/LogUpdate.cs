using System;
using System.Collections.Generic;

namespace ChecklistAngular.Models
{
    public partial class LogUpdate
    {
        public LogUpdate()
        {
            LogUpdateHistory = new HashSet<LogUpdateHistory>();
            LogUpdateSteps = new HashSet<LogUpdateSteps>();
            
        }

        public int Idupdate { get; set; }
        public string ProdLine { get; set; }
        public string SiteKml { get; set; }
        public short? UpdateNum { get; set; }
        public string System { get; set; }
        public string Process { get; set; }
        public int? Task { get; set; }
        public string UpdateRelease { get; set; }
        public string UpdatePpack { get; set; }
        public string Note { get; set; }
        public int Idchecklist { get; set; }
        public short Version { get; set; }
        public string Status { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        

        public ICollection<LogUpdateHistory> LogUpdateHistory { get; set; }
        public ICollection<LogUpdateSteps> LogUpdateSteps { get; set; }
    }
}
