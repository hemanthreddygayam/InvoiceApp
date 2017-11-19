using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApplication.Models
{
    public class InvoiceSearchViewModel
    {
        public string Status { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int Results { get; set; }
        public int Page { get; set; }
    }
}
