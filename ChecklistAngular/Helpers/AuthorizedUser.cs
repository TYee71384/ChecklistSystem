using ChecklistAngular.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChecklistAngular.Helpers
{
    public class AuthorizedUser : AuthorizationHandler<UserAccess>
    {
        private SWAT_UpdateChecklistsContext ctx;
        public AuthorizedUser(SWAT_UpdateChecklistsContext ctx)
        {
            this.ctx = ctx;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserAccess user)
        {
            
            foreach(var u in ctx.ListUserApprove)
            {
                if (user.User.ToUpper() == u.UserApprove.ToString())
                     context.Succeed(user);

               
            }
                 return Task.CompletedTask;

        }
    }
}
