using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApplication.DataAccessLayer
{
    public class DbUserModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public Int64 CategoryId { get; set; }
        public long UserId { get; set; }


    }
}
