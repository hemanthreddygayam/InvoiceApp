using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApplication.DataAccessLayer
{
    public class DbVesselDocument
    {
        public int FileId { get; set; }
        public long InvoiceId { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string controller { get; set; }
    }
}
