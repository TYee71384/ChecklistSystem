using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ChecklistAngular.Models
{
    public partial class LogChecklistHistory
    {
        public int Idchecklist { get; set; }
        public short Version { get; set; }
        public DateTime FileTime { get; set; }
        public string FileBy { get; set; }
        public string FileAction { get; set; }
        public string Status { get; set; }

        [JsonIgnore]
        public LogChecklistIndex IdchecklistNavigation { get; set; }
        public LogChecklist LogChecklist { get; set; }
    }
}
