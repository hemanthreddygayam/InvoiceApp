using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApplication.Models
{
    public class InvoicePagingResults
    {
        public List<InvoiceResults> invoiceResults { get; set; }
        public int totalNumberOfRecords { get; set; }
        public int currentPageNumber { get; set; }
        public InvoiceSearchViewModel modelInvoice { get; set; }
    }
}
