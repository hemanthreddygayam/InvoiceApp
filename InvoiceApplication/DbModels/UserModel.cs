using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApplication.DbModels
{
    public class UserModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public int CategoryId { get; set; }

    }
}
