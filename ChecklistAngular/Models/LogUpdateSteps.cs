using System;
using System.Collections.Generic;

namespace ChecklistAngular.Models
{
    public partial class LogUpdateSteps
    {
        public int Idupdate { get; set; }
        public short Step { get; set; }
        public string StepText { get; set; }
        public string Comment { get; set; }
        public string Progress { get; set; }

        public LogUpdate IdupdateNavigation { get; set; }
    }
}
