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
        private TrackingDbContext _context;

        public IsApprovingAuthority(TrackingDbContext context)
        {
            _context = context;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ApproverRequirement requirement)
        {
            var username = context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (_context.Btsuser.Where(e => e.UserName == username).Any(e => e.CategoryId == 2))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}