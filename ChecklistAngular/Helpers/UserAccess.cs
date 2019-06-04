using Microsoft.AspNetCore.Authorization;
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
            User = WindowsIdentity.GetCurrent().Name;
            User = User.Substring(User.IndexOf(@"\") + 1);
        }
    }
}
