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
        private TrackingDbContext _context;

        public IsCheckingAuthority(TrackingDbContext context)
        {
            _context = context;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CheckingAuthorityRequirement requirement)
        {
            var employee_email = context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (_context.Btsuser.Where(e => e.UserName == employee_email).Any(e => e.CategoryId == 2))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
