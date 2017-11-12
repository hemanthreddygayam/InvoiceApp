using System;
using System.Collections.Generic;

namespace InvoiceApplication.DbModels
{
    public partial class VesselDocuments
    {
        public int FileId { get; set; }
        public long InvoiceId { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
    }
}
