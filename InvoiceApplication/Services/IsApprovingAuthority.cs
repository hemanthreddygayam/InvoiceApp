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
    public class IsApprovingAuthority : AuthorizationHandler<ApproverRequirement>
    {
        private DbHelper _helper;

        public IsApprovingAuthority()
        {
            _helper = new DbHelper();
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ApproverRequirement requirement)
        {
            if (context.User.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                var username = context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                IDBService dbService = new DBservice(_helper);
                var user = dbService.FetchUser(username);
                if (user != null && user.CategoryId ==  2)
                {
                    context.Succeed(requirement);
                }
            }
                return Task.CompletedTask;
            
        }
    }
}