using System;
using System.Collections.Generic;

namespace InvoiceApplication.DbModels
{
    public partial class Btsuser
    {
        public long UserId { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public string Designation { get; set; }
        public long CategoryId { get; set; }
    }
}
