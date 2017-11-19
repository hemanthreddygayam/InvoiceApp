using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApplication.Models
{
    public class InvoiceResults
    {
        public long invoiceId;
        public string invoiceNo;
        public string invoiceDate;
        public string dueDate;
        public string currencyCode;
        public decimal totalAmt;
        public string customerName;
    }
}
