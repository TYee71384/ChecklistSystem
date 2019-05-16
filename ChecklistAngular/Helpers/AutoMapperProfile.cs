using AutoMapper;
using ChecklistAngular.DTOs;
using ChecklistAngular.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChecklistAngular.Helpers
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<LogUpdate, Report>();
            CreateMap<LogChecklist, ChecklistDescription>().ReverseMap();
        }
    }
}
