using InvoiceApplication.DataAccessLayer;
using InvoiceApplication.DbModels;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InvoiceApplication.Services
{
    public class IsCheckingAuthority : AuthorizationHandler<CheckingAuthorityRequirement>
    {
        public DbHelper _helper;
        public IsCheckingAuthority()
        {
            _helper = new DbHelper();
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CheckingAuthorityRequirement requirement)
        {
            if (context.User.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                var username = context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                IDBService service = new DBservice(_helper);
                var user = service.FetchUser(username);
                if (user != null && user.CategoryId == 1)
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }
}
