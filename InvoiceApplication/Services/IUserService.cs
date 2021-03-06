﻿using InvoiceApplication.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApplication.Services
{
    public interface IUserService
    {
        Task<bool> ValidateCredentials(string username, string password, out DbUserModel user);
    }
}
