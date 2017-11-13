using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApplication.Models
{
    public class InvoiceApprovalModel
    {
        public string status { get; set; }
        public long InvoiceId { get; set; }
    }
}
