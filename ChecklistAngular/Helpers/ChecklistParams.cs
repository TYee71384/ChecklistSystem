using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChecklistAngular.Helpers
{
    public class ChecklistParams
    {
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 10;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }
        public string Platform { get; set; }
        public string System { get; set; }
        public string Process { get; set; }
        public string Release { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Keyword { get; set; }

    }
}
