using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApplication.Models
{
    public class InvoiceViewModel
    {
        public long invoiceId { get; set; }
        public long branchId { get; set; }
        public string invoiceNumber { get; set; }
        public DateTime invoiceDate { get; set; }
        public DateTime DelivaryDate { get; set; }
        public DateTime DueDate { get; set; }
        public string currencyCode { get; set; }
        public decimal Amount { get; set; }
        public int exchangeRate { get; set; }
        public decimal totalLocalAmount { get; set; }
        public int customerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime AccountDate { get; set; }
        public bool showbuttons {get;set;}
   
       
     
        
    }
}
