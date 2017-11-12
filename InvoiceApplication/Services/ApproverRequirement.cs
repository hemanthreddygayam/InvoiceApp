using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApplication.Services
{
    public class ApproverRequirement:IAuthorizationRequirement
    {
    }
}
