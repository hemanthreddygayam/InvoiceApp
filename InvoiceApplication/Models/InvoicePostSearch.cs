using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApplication.Models
{
    public class InvoicePostSearch
    {
        public string Status { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}
