using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace ChecklistAngular.Helpers
{
    public class UserAccess : IAuthorizationRequirement
    {
        public string User { get; set; }

        public UserAccess()
        {

        }
    }
}
