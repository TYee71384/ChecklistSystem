using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChecklistAngular.Models
{
    public class Report
    {
        public int Idupdate { get; set; }
        public string ProdLine { get; set; }
        public string SiteKml { get; set; }
        public string UpdateNum { get; set; }
        public DateTime StartTime { get; set; }
        public string Status { get; set; }
        public string Percentage { get; set; }
    }
}
