using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ChecklistAngular.Models
{
    public partial class LogUpdateHistory
    {
        public int Idupdate { get; set; }
        public DateTime FileTime { get; set; }
        public string FileBy { get; set; }
        public string FileAction { get; set; }
        public string Status { get; set; }

        [JsonIgnore]
        public LogUpdate IdupdateNavigation { get; set; }
    }
}
