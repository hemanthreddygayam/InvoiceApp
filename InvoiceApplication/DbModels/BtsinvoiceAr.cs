using System;
using System.Collections.Generic;

namespace InvoiceApplication.DbModels
{
    public partial class BtsinvoiceAr
    {
        public long BranchId { get; set; }
        public long InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime AccountDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime DueDate { get; set; }
        public string CurrencyCode { get; set; }
        public int ExRate { get; set; }
        public string CreditTerms { get; set; }
        public decimal TotalAmt { get; set; }
        public decimal TotalLocalAmt { get; set; }
        public int CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string BillingAddress1 { get; set; }
        public string BillingAddress2 { get; set; }
        public string BillingCountry { get; set; }
        public bool? IsWaitingApproval { get; set; }
        public bool? IsChecked { get; set; }
        public bool? IsCheckedPending { get; set; }
        public string CheckedBy { get; set; }
        public string CheckedDate { get; set; }
        public bool? IsApproved { get; set; }
        public bool? IsApprovalPending { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string EditBy { get; set; }
        public DateTime? EditDate { get; set; }
    }
}
