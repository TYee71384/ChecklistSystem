using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ChecklistAngular.Models
{
    public partial class LogChecklistSteps
    {
        public int Idchecklist { get; set; }
        public int Idstep { get; set; }
        public short Version { get; set; }
        public short Step { get; set; }
        public string StepText { get; set; }
        public string Title { get; set; }

        public bool IsRequired { get; set; }

        [JsonIgnore]
        public LogChecklistIndex IdchecklistNavigation { get; set; }
        public LogChecklist LogChecklist { get; set; }
    }
}
