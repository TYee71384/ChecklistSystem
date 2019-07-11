using ChecklistAngular.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private readonly IHttpContextAccessor httpContextAccessor;

        public AuthorizedUser(SWAT_UpdateChecklistsContext ctx, IHttpContextAccessor httpContextAccessor)
        {
            this.ctx = ctx;
            this.httpContextAccessor = httpContextAccessor;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserAccess user)
        {
            var newuser = httpContextAccessor.HttpContext.User.Identity.Name;
            newuser = newuser.Substring(newuser.IndexOf(@"\") + 1);
            foreach (var u in ctx.ListUserApprove)
            {
                if (newuser.ToUpper() == u.UserApprove.ToString())
                {
                    user.User = newuser;
                context.Succeed(user);
                }
                     

               
            }
                 return Task.CompletedTask;

        }
    }
}
