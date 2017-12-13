using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApplication.Models
{
    public class InvoiceResults
    {
        public long InvoiceId { get; set; } 
        public string InvoiceNo { get; set; }
        public string CurrencyCode { get; set; }
        public string TotalAmt { get; set; }
        public string CustomerName { get; set; }
        public string VesselName { get; set; }
        public string AccountDate { get; set; }
        public int ExchangeRate { get; set; }
        public string DueDate { get; set; }
    }
}
