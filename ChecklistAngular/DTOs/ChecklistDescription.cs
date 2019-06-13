using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChecklistAngular.DTOs
{
    public class ChecklistDescription
    {
        public int Idchecklist { get; set; }
        public short Version { get; set; }
        public string Title { get; set; }
        public string ProdLine { get; set; }
        public string System { get; set; }
        public string Process { get; set; }
        public string Rel { get; set; }
        public string Type { get; set; }
        public string Scope { get; set; }
    }
}
