using InvoiceApplication.DataAccessLayer;
using InvoiceApplication.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApplication.Services
{
    public class UserService : IUserService
    {
        public DbHelper dbHelper;
        public UserService()
        {
            dbHelper = new DbHelper();
        }
        public Task<bool> ValidateCredentials(string username, string password, out DbUserModel user)
        {
            user = null;
            IDBService _dbService = new DBservice(dbHelper);
            var verifyUser =  _dbService.FetchUser(username);
            if (verifyUser != null)
            {
                user = verifyUser;
                return Task.FromResult(true);
            }
            return Task.FromResult(false);

        }
    }
}
