using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static InvoiceApplication.Utilies;

namespace InvoiceApplication.Models
{
    public class InvoiceApprovalModel
    {
        public InvoiceStatus Status { get; set; }
        public string PreviousStatus { get; set; }
        public long InvoiceId { get; set; }
        public string Remarks { get; set; }
    }
}
