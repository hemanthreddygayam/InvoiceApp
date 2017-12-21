using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApplication.Models
{
    public class InvoiceViewModel
    {
        public long InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public string VesselName { get; set; }
        public string AccountDate { get; set; }
        public string DelivaryDate { get; set; }
        public DateTime DueDate { get; set; }
        public string CurrencyCode { get; set; }
        public string Amount { get; set; }
        public int ExchangeRate { get; set; }
        public string TotalLocalAmount { get; set; }
        public string CustomerName { get; set; }
        public bool Showbuttons {get;set;}
        public int NoOfDocuments { get; set; }
        public string InvoiceStatus { get; set; }
        public string Remarks { get; set; }
   
       
     
        
    }
}
