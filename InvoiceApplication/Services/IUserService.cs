using InvoiceApplication.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApplication.Services
{
    interface IUserService
    {
        Task<bool> ValidateCredentials(string username, string password, out Btsuser user);
    }
}
