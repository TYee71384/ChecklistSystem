using ChecklistAngular.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChecklistAngular.DTOs
{
    public class UpdateSearch
    {
        public int Idupdate { get; set; }
        public string ProdLine { get; set; }
        public string SiteKml { get; set; }
        public short? UpdateNum { get; set; }
        public string System { get; set; }
        public string Process { get; set; }
        public int? Task { get; set; }
        public string UpdateRelease { get; set; }
        public string UpdatePpack { get; set; }
        public int Idchecklist { get; set; }
        public short Version { get; set; }
        public string Status { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Percentage { get; set; }

        [JsonIgnore]
        public ICollection<LogUpdateHistory> LogUpdateHistory { get; set; }
       
        [JsonIgnore]
        public ICollection<LogUpdateSteps> LogUpdateSteps { get; set; }



    }
}
