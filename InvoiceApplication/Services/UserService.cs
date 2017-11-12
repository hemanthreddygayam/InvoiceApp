using InvoiceApplication.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApplication.Services
{
    public class UserService : IUserService
    {
        private TrackingDbContext _context;

        public UserService(TrackingDbContext context)
        {
            _context = context;
        }
        public Task<bool> ValidateCredentials(string username, string password, out Btsuser user)
        {
            user = null;
            var verifyUser = _context.Btsuser.FirstOrDefault(e => e.UserName == username && e.IsActive == true);
            if (verifyUser != null)
            {
                if (BCrypt.Net.BCrypt.Verify(password, verifyUser.Password)) //Verify Password
                {
                    user = verifyUser;
                    return Task.FromResult(true);
                }
            }
            return Task.FromResult(false);
        }
    }
}
