using ChecklistAngular.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChecklistAngular.Helpers
{
    public class Helpers
    {

        public static string GetPercentage(LogUpdate i)
        {
            var total = i.LogUpdateSteps.Count();
            var count = i.LogUpdateSteps.Where(x => x.Progress == "Done" || x.Progress == "Skip").Count();
            return string.Format("{0:0.0%}", (float)count / total);
        }

       
    }
}
